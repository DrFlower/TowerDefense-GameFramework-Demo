using UnityEditor;

namespace ActionGameFramework.Projectiles.Editor
{
	[CustomEditor(typeof(BallisticProjectile))]
	public class BallisticProjectileEditor : UnityEditor.Editor
	{
		protected SerializedProperty m_FireModeSelector;
		protected SerializedProperty m_StartSpeedSelector;
		protected SerializedProperty m_StartAngleSelector;
		protected SerializedProperty m_ArcPreferenceSelector;

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(m_ArcPreferenceSelector);

			EditorGUILayout.LabelField("Choose whether FireAtPoint is calculated based on set angle or set velocity.");
			EditorGUILayout.PropertyField(m_FireModeSelector);

			if (m_FireModeSelector.enumValueIndex == (int) BallisticFireMode.UseLaunchSpeed)
			{
				EditorGUILayout.PropertyField(m_StartSpeedSelector);
			}
			else
			{
				EditorGUILayout.PropertyField(m_StartAngleSelector);
			}

			serializedObject.ApplyModifiedProperties();
		}

		void OnEnable()
		{
			m_FireModeSelector = serializedObject.FindProperty("fireMode");
			m_StartAngleSelector = serializedObject.FindProperty("firingAngle");
			m_StartSpeedSelector = serializedObject.FindProperty("startSpeed");
			m_ArcPreferenceSelector = serializedObject.FindProperty("arcPreference");
		}
	}
}