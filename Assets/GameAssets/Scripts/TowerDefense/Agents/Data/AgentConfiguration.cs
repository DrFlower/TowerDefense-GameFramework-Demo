using UnityEngine;

namespace TowerDefense.Agents.Data
{
	[CreateAssetMenu(fileName = "AgentConfiguration.asset", menuName = "TowerDefense/Agent Configuration", order = 1)]
	public class AgentConfiguration : ScriptableObject
	{
		/// <summary>
		/// The name of the agent
		/// </summary>
		public string agentName;

		/// <summary>
		/// Short summary of the agent
		/// </summary>
		[Multiline]
		public string agentDescription;

		/// <summary>
		/// The Agent prefab that will be used on instantiation
		/// </summary>
		public Agent agentPrefab;
	}
}