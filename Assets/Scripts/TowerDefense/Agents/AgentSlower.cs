using System.Collections.Generic;
using Core.Health;
using Core.Utilities;
using UnityEngine;

namespace TowerDefense.Agents
{
	/// <summary>
	/// This effect will get attached to an agent that is within range of the SlowAffector radius
	/// </summary>
	public class AgentSlower : AgentEffect
	{
		protected GameObject m_SlowFx;

		protected List<float> m_CurrentEffects = new List<float>();

		/// <summary>
		/// Initializes the slower with the parameters configured in the SlowAffector
		/// </summary>
		/// <param name="slowFactor">Normalized float that represents the % slowdown applied to the agent</param>
		/// <param name="slowfxPrefab">The instantiated object to visualize the slow effect</param>
		/// <param name="position"></param>
		/// <param name="scale"></param>
		public void Initialize(float slowFactor, GameObject slowfxPrefab = null, 
		                       Vector3 position = default(Vector3),
		                       float scale = 1)
		{
			LazyLoad();
			m_CurrentEffects.Add(slowFactor);

			// find greatest slow effect
			float min = slowFactor;
			foreach (float item in m_CurrentEffects)
			{
				min = Mathf.Min(min, item);
			}
			
			float originalSpeed = m_Agent.originalMovementSpeed;
			float newSpeed = originalSpeed * min;
			m_Agent.navMeshNavMeshAgent.speed = newSpeed;

			if (m_SlowFx == null && slowfxPrefab != null)
			{
				m_SlowFx = Poolable.TryGetPoolable(slowfxPrefab);
				m_SlowFx.transform.parent = transform;
				m_SlowFx.transform.localPosition = position;
				m_SlowFx.transform.localScale *= scale;
			}
			m_Agent.removed += OnRemoved;
		}

		/// <summary>
		/// Resets the agent's speed 
		/// </summary>
		public void RemoveSlow(float slowFactor)
		{
			m_Agent.removed -= OnRemoved;
			
			m_CurrentEffects.Remove(slowFactor);
			if (m_CurrentEffects.Count != 0)
			{
				return;
			}
			
			// No more slow effects
			ResetAgent();
		}

		/// <summary>
		/// Agent has died, remove affect
		/// </summary>
		void OnRemoved(DamageableBehaviour targetable)
		{
			m_Agent.removed -= OnRemoved;
			ResetAgent();
		}

		void ResetAgent()
		{
			if (m_Agent != null)
			{
				m_Agent.navMeshNavMeshAgent.speed = m_Agent.originalMovementSpeed;
			}
			if (m_SlowFx != null)
			{
				Poolable.TryPool(m_SlowFx);
				m_SlowFx.transform.localScale = Vector3.one;
				m_SlowFx = null;
			}
			Destroy(this);
		}
	}
}