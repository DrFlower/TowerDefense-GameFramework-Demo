using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Nodes
{
	/// <summary>
	/// Provides a way to select a node for agents to navigate towards
	/// </summary>
	public abstract class NodeSelector : MonoBehaviour
	{
		/// <summary>
		/// A list of Nodes that can be selected by this NodeSelector
		/// </summary>
		public List<Node> linkedNodes;

		/// <summary>
		/// Gets the next node in the fixed list of nodes
		/// </summary>
		/// <returns>The next node in the list of Nodes, null if the node is the endpoint</returns>
		public abstract Node GetNextNode();

#if UNITY_EDITOR
		/// <summary>
		/// Draws the links between nodes for editor purposes
		/// </summary>
		protected virtual void OnDrawGizmos()
		{
			if (linkedNodes == null)
			{
				return;
			}
			int count = linkedNodes.Count;
			for (int i = 0; i < count; i++)
			{
				Node node = linkedNodes[i];
				if (node != null)
				{
					Gizmos.DrawLine(transform.position, node.transform.position);
				}
			}
		}
#endif
	}
}