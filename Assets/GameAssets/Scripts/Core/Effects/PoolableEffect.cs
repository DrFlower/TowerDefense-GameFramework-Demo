using System.Collections.Generic;
using Core.Utilities;
using UnityEngine;

namespace TowerDefense.Effects
{
	/// <summary>
	/// Simple effect support script to reset trails and particles on enable, and also
	/// stops and starts reused emitters (to prevent them emitting when moving after being repooled)
	/// </summary>
	public class PoolableEffect : Poolable
	{
		protected List<ParticleSystem> m_Systems;
		protected List<TrailRenderer> m_Trails;

		bool m_EffectsEnabled;
		
		/// <summary>
		/// Stop emitting all particles
		/// </summary>
		public void StopAll()
		{
			foreach (var particleSystem in m_Systems)
			{
				particleSystem.Stop();
			}
		}
		
		/// <summary>
		/// Turn off all known systems
		/// </summary>
		public void TurnOffAllSystems()
		{
			if (!m_EffectsEnabled)
			{
				return;
			}
			
			// Reset all systems and trails
			foreach (var particleSystem in m_Systems)
			{
				particleSystem.Clear();
				var emission = particleSystem.emission;
				emission.enabled = false;
			}

			foreach (var trailRenderer in m_Trails)
			{
				trailRenderer.Clear();
				trailRenderer.enabled = false;
			}

			m_EffectsEnabled = false;
		}

		/// <summary>
		/// Turn on all known systems
		/// </summary>
		public void TurnOnAllSystems()
		{
			if (m_EffectsEnabled)
			{
				return;
			}
			
			// Re-enable all systems and trails
			foreach (var particleSystem in m_Systems)
			{
				particleSystem.Clear();
				var emission = particleSystem.emission;
				emission.enabled = true;
			}

			foreach (var trailRenderer in m_Trails)
			{
				trailRenderer.Clear();
				trailRenderer.enabled = true;
			}

			m_EffectsEnabled = true;
		}

		protected override void Repool()
		{
			base.Repool();
			TurnOffAllSystems();
		}

		protected virtual void Awake()
		{
			m_EffectsEnabled = true;
			
			// Cache systems and trails, but only active and emitting ones
			m_Systems = new List<ParticleSystem>();
			m_Trails = new List<TrailRenderer>();

			foreach (var system in GetComponentsInChildren<ParticleSystem>())
			{
				if (system.emission.enabled && system.gameObject.activeSelf)
				{
					m_Systems.Add(system);
				}
			}
			
			foreach (var trail in GetComponentsInChildren<TrailRenderer>())
			{
				if (trail.enabled && trail.gameObject.activeSelf)
				{
					m_Trails.Add(trail);
				}
			}

			TurnOffAllSystems();
		}
	}
}