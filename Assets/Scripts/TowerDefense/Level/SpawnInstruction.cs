using System;
using TowerDefense.Agents.Data;
using TowerDefense.Nodes;
using UnityEngine;

namespace TowerDefense.Level
{
	/// <summary>
	/// Serializable class for specifying properties of spawning an agent
	/// </summary>
	[Serializable]
	public class SpawnInstruction
	{
		/// <summary>
		/// The agent to spawn - i.e. the monster for the wave
		/// </summary>
		public AgentConfiguration agentConfiguration;

		/// <summary>
		/// The delay from the previous spawn until when this agent is spawned
		/// </summary>
		[Tooltip("The delay from the previous spawn until when this agent is spawned")]
		public float delayToSpawn;

		/// <summary>
		/// The starting node, where the agent is spawned
		/// </summary>
		public Node startingNode;
	}
}