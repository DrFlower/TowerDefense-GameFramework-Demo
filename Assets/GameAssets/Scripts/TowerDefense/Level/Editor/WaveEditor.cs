using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace TowerDefense.Level.Editor
{
	/// <summary>
	/// Custom editor to display wave time sum
	/// </summary>
	[CustomEditor(typeof(Wave), true)]
	public class WaveEditor : UnityEditor.Editor
	{
		Wave m_Wave;

		void OnEnable()
		{
			m_Wave = (Wave) target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			// Draw a summary of all spawn instructions
			List<SpawnInstruction> spawnInstructions = m_Wave.spawnInstructions;
			if (spawnInstructions == null)
			{
				return;
			}
			
			// Count spawn instructions
			float lastSpawnTime = spawnInstructions.Sum(t => t.delayToSpawn);

			// Group by enemy type so we can count per type as well
			var groups = spawnInstructions.GroupBy(t => t.agentConfiguration);
			var groupCounts = groups.Select(g => new {Number = g.Count(), Item = g.Key.agentName});

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Wave summary");

			EditorGUILayout.LabelField(string.Format("Last spawn time: {0}", lastSpawnTime));
			EditorGUILayout.Space();
			foreach (var groupCount in groupCounts)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(string.Format("Enemy:\t{0}", groupCount.Item));
				EditorGUILayout.LabelField(string.Format("Count:\t{0}", groupCount.Number));
				EditorGUILayout.EndHorizontal();
			}
		}
	}
}