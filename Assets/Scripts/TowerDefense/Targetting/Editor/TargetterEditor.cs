using UnityEditor;
using UnityEngine;

namespace TowerDefense.Targetting.Editor
{
	/// <summary>
	/// The editor for configuring targetter
	/// </summary>
	[CustomEditor(typeof(Targetter)), CanEditMultipleObjects]
	public class TargetterEditor : UnityEditor.Editor
	{
		/// <summary>
		/// Configuration for which collider to use
		/// </summary>
		public enum TargetterCollider
		{
			/// <summary>
			/// For sphere collider
			/// </summary>
			Sphere,

			/// <summary>
			/// For capsule collider
			/// </summary>
			Capsule
		}

		/// <summary>
		/// The targetter to edit
		/// </summary>
		Targetter m_Targetter;

		/// <summary>
		/// The collision configuration to use
		/// </summary>
		TargetterCollider m_ColliderConfiguration;

		/// <summary>
		/// The radius of the collider
		/// </summary>
		float m_ColliderRadius;

		// Capsule specific info

		/// <summary>
		/// The height of a capsule collider
		/// </summary>
		float m_ExtraVerticalRange;

		/// <summary>
		/// The attached collider
		/// </summary>
		Collider m_AttachedCollider;

		/// <summary>
		/// The serialized property representing <see cref="m_AttachedCollider"/>
		/// </summary>
		SerializedProperty m_SerializedAttachedCollider;

		/// <summary>
		/// draws the default inspector 
		/// and then draws configuration for colliders
		/// </summary>
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			// To make the inspector a little bit neater
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Targetter Collider Configuration", EditorStyles.boldLabel);

			m_ColliderConfiguration =
				(TargetterCollider) EditorGUILayout.EnumPopup("Targetter Collider", m_ColliderConfiguration);
			AttachCollider();
			m_ColliderRadius = EditorGUILayout.FloatField("Radius", m_ColliderRadius);
			if (m_ColliderConfiguration == TargetterCollider.Capsule)
			{
				m_ExtraVerticalRange = EditorGUILayout.FloatField("Vertical Range", m_ExtraVerticalRange);
			}
			SetValues();
			EditorUtility.SetDirty(m_Targetter);
			EditorUtility.SetDirty(m_AttachedCollider);
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// For attaching and hiding the correct collider
		/// </summary>
		void AttachCollider()
		{
			switch (m_ColliderConfiguration)
			{
				case TargetterCollider.Sphere:
					if (m_AttachedCollider is SphereCollider)
					{
						GetValues();
						return;
					}
					if (m_AttachedCollider != null)
					{
						DestroyImmediate(m_AttachedCollider, true);
					}
					m_AttachedCollider = m_Targetter.gameObject.AddComponent<SphereCollider>();
					m_SerializedAttachedCollider.objectReferenceValue = m_AttachedCollider;
					break;
				case TargetterCollider.Capsule:
					if (m_AttachedCollider is CapsuleCollider)
					{
						GetValues();
						return;
					}
					if (m_AttachedCollider != null)
					{
						DestroyImmediate(m_AttachedCollider, true);
					}
					m_AttachedCollider = m_Targetter.gameObject.AddComponent<CapsuleCollider>();
					m_SerializedAttachedCollider.objectReferenceValue = m_AttachedCollider;
					break;
			}
			SetValues();
			m_AttachedCollider.hideFlags = HideFlags.HideInInspector;
		}

		/// <summary>
		/// Assigns the values to the collider
		/// </summary>
		void SetValues()
		{
			switch (m_ColliderConfiguration)
			{
				case TargetterCollider.Sphere:
					var sphere = (SphereCollider) m_AttachedCollider;
					sphere.radius = m_ColliderRadius;
					break;
				case TargetterCollider.Capsule:
					var capsule = (CapsuleCollider) m_AttachedCollider;
					capsule.radius = m_ColliderRadius;
					capsule.height = m_ExtraVerticalRange + m_ColliderRadius * 2;
					break;
			}
		}

		/// <summary>
		/// Obtains the information from the collider
		/// </summary>
		void GetValues()
		{
			switch (m_ColliderConfiguration)
			{
				case TargetterCollider.Sphere:
					var sphere = (SphereCollider) m_AttachedCollider;
					m_ColliderRadius = sphere.radius;
					break;
				case TargetterCollider.Capsule:
					var capsule = (CapsuleCollider) m_AttachedCollider;
					m_ColliderRadius = capsule.radius;
					m_ExtraVerticalRange = capsule.height - m_ColliderRadius * 2;
					break;
			}
		}

		/// <summary>
		/// Caches the collider and hides it
		/// and configures all the necessary information from it
		/// </summary>
		void OnEnable()
		{
			m_Targetter = (Targetter) target;
			m_SerializedAttachedCollider = serializedObject.FindProperty("attachedCollider");
			m_AttachedCollider = (Collider) m_SerializedAttachedCollider.objectReferenceValue;

			if (m_AttachedCollider == null)
			{
				m_AttachedCollider = m_Targetter.GetComponent<Collider>();
				if (m_AttachedCollider == null)
				{
					switch (m_ColliderConfiguration)
					{
						case TargetterCollider.Sphere:
							m_AttachedCollider = m_Targetter.gameObject.AddComponent<SphereCollider>();
							break;
						case TargetterCollider.Capsule:
							m_AttachedCollider = m_Targetter.gameObject.AddComponent<CapsuleCollider>();
							break;
					}
					m_SerializedAttachedCollider.objectReferenceValue = m_AttachedCollider;
				}
			}
			if (m_AttachedCollider is SphereCollider)
			{
				m_ColliderConfiguration = TargetterCollider.Sphere;
			}
			else if (m_AttachedCollider is CapsuleCollider)
			{
				m_ColliderConfiguration = TargetterCollider.Capsule;
			}
			// to ensure the collider is referenced by the serialized object
			if (m_SerializedAttachedCollider.objectReferenceValue == null)
			{
				m_SerializedAttachedCollider.objectReferenceValue = m_AttachedCollider;
			}
			GetValues();
			m_AttachedCollider.isTrigger = true;
			m_AttachedCollider.hideFlags = HideFlags.HideInInspector;
		}
	}
}