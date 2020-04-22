using Core.Extensions;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace TowerDefense.Nodes
{
	/// <summary>
	/// Randomly selects the next node
	/// </summary>
	public class RandomNodeSelector : NodeSelector
	{
		/// <summary>
		/// The sum of all Node weights in m_LinkedNodes
		/// </summary>
		protected int m_WeightSum;

		/// <summary>
		/// Gets a random node in the list
		/// </summary>
		/// <returns>The randomly selected node</returns>
		public override Node GetNextNode()
		{
			if (linkedNodes == null)
			{
				return null;
			}
			int totalWeight = m_WeightSum;
			return linkedNodes.WeightedSelection(totalWeight, t => t.weight);
		}

		protected void Awake()
		{
			// cache the linked node weights
			m_WeightSum = TotalLinkedNodeWeights();
		}
#if UNITY_EDITOR
		protected override void OnDrawGizmos()
		{
			Gizmos.color = Color.cyan;
			base.OnDrawGizmos();
		}
#endif
		/// <summary>
		/// Sums up the weights of the linked nodes for random selection
		/// </summary>
		/// <returns>Weight Sum of Linked Nodes</returns>
		protected int TotalLinkedNodeWeights()
		{
			int totalWeight = 0;
			int count = linkedNodes.Count;
			for (int i = 0; i < count; i++)
			{
				totalWeight += linkedNodes[i].weight;
			}
			return totalWeight;
		}
	}
}