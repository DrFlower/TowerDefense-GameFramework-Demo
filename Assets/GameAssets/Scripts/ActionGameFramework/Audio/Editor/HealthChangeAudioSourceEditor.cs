using UnityEditor;
using UnityEngine;

namespace ActionGameFramework.Audio.Editor
{
	/// <summary>
	/// Custom editor for <see cref="HealthChangeAudioSource"/> that sorts sounds on the fly
	/// </summary>
	[CustomEditor(typeof(HealthChangeAudioSource))]
	public class HealthChangeAudioSourceEditor : UnityEditor.Editor
	{
		protected const string k_HelpMessage =
			"This list needs to be sorted in order " +
			"for sounds to be played correctly" +
			"\nList will sort automatically when this component is deselected." +
			"\nYou can also press the \'Sort\' button once you are done editing the sound list.";

		/// <summary>
		/// The <see cref="HealthChangeAudioSource"/> that is selected
		/// </summary>
		protected HealthChangeAudioSource m_Source;

		/// <summary>
		/// Sort the sounds when the <see cref="HealthChangeAudioSource"/> is selected
		/// </summary>
		protected void OnEnable()
		{
			m_Source = target as HealthChangeAudioSource;
		}

		/// <summary>
		/// Sort the sounds when <see cref="HealthChangeAudioSource"/> is deselected
		/// </summary>
		protected void OnDisable()
		{
			Sort();
		}

		/// <summary>
		/// Sort the <see cref="HealthChangeAudioSource"/>'s sound list
		/// </summary>
		protected void Sort()
		{
			if (m_Source != null)
			{
				m_Source.Sort();
				EditorUtility.SetDirty(m_Source);
			}
		}

		/// <summary>
		/// Provide a button to manually sort sounds that were edited
		/// </summary>
		public override void OnInspectorGUI()
		{
			EditorGUILayout.HelpBox(k_HelpMessage, MessageType.Info);
			base.OnInspectorGUI();
			if (GUILayout.Button("Sort"))
			{
				Sort();
			}
		}
	}
}