using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Utilities.Editor
{
	/// <summary>
	/// Property drawer for serializable interfaces
	/// </summary>
	[CustomPropertyDrawer(typeof(SerializableInterface), true)]
	public class SerializableInterfaceDrawer : PropertyDrawer
	{
		/// <summary>
		/// Cached interface type that we get generically
		/// </summary>
		Type m_CachedInterfaceType;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty gameObjectProperty = property.FindPropertyRelative("unityObjectReference");

			// Try and find the interface type we need to filter for
			// Use ISerializableInterface by default
			Type interfaceType = typeof(ISerializableInterface);
			
			Object containingObject = property.serializedObject.targetObject;
			Type containingObjectType = property.serializedObject.targetObject.GetType();
			FieldInfo field = GetNestedField(containingObjectType, property.propertyPath);

			if (field != null)
			{
				Type serializableInterfaceType = FindAncestorSerializableType(field.FieldType);
				interfaceType = serializableInterfaceType.GetGenericArguments()[0];
			}
			
			EditorGUI.BeginProperty(position, label, property);
			EditorGUI.ObjectField(position, gameObjectProperty, interfaceType, 
			                      new GUIContent(property.displayName));
			EditorGUI.EndProperty();
		}

		static FieldInfo GetNestedField(Type owningType, string fieldPath)
		{
			while (true)
			{
				int firstDotIndex = fieldPath.IndexOf(".", StringComparison.Ordinal);
				if (firstDotIndex > 0)
				{
					// Get first type and recurse in
					string parentFieldName = fieldPath.Substring(0, firstDotIndex);
					FieldInfo parentField = owningType.GetField(parentFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

					if (parentField == null)
					{
						return null;
					}
					owningType = parentField.FieldType;
					fieldPath = fieldPath.Substring(firstDotIndex + 1);
				}
				else
				{
					return owningType.GetField(fieldPath, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				}
			}
		}

		static Type FindAncestorSerializableType(Type childType)
		{
			while (childType != null &&
			       !childType.IsGenericType &&
			       childType != typeof(SerializableInterface<>))
			{
				childType = childType.BaseType;
			}

			return childType;
		}
	}
}