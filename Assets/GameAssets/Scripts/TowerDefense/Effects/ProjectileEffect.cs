using ActionGameFramework.Projectiles;
using Core.Utilities;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Effects
{
	/// <summary>
	/// Class for spawning and managing effects on this projectile. Used for effects that should persist
	/// a little longer after a projectile is destroyed/repooled. Creates the effect on enable, moves it to 
	/// follow us every frame while we're active.
	/// 
	/// On disable, it'll try and find a SelfDestroyTimer on the effect to trigger its destruction, otherwise
	/// repools it immediately.
	/// </summary>
	[RequireComponent(typeof(IProjectile))]
	public class ProjectileEffect : MonoBehaviour
	{
		/// <summary>
		/// Preafb that gets spawned when this projectile fires
		/// </summary>
		public GameObject effectPrefab;

		/// <summary>
		/// Transform the effect follows
		/// </summary>
		public Transform followTransform;

		/// <summary>
		/// Cached spawned effect
		/// </summary>
		GameObject m_SpawnedEffect;
		
		/// <summary>
		/// Cached destruction timer on the spawned object
		/// </summary>
		SelfDestroyTimer m_DestroyTimer;

		/// <summary>
		/// Cached poolable effect on the spawned object
		/// </summary>
		PoolableEffect m_Resetter;

		/// <summary>
		/// Cached projectile
		/// </summary>
		IProjectile m_Projectile;

		/// <summary>
		/// Register projectile fire events
		/// </summary>
		protected virtual void Awake()
		{
			m_Projectile = GetComponent<IProjectile>();
			m_Projectile.fired += OnFired;
			if (followTransform != null)
			{
				followTransform = transform;
			}
		}

		/// <summary>
		/// Unregister delegates
		/// </summary>
		protected virtual void OnDestroy()
		{
			m_Projectile.fired -= OnFired;
		}

		/// <summary>
		/// Spawn our effect
		/// </summary>
		protected virtual void OnFired()
		{
			if (effectPrefab != null)
			{
				m_SpawnedEffect = Poolable.TryGetPoolable(effectPrefab);
				m_SpawnedEffect.transform.parent = null;
				m_SpawnedEffect.transform.position = followTransform.position;
				m_SpawnedEffect.transform.rotation = followTransform.rotation;
				
				// Make sure to disable timer if it's on initially, so it doesn't destroy this object
				m_DestroyTimer = m_SpawnedEffect.GetComponent<SelfDestroyTimer>();
				if (m_DestroyTimer != null)
				{
					m_DestroyTimer.enabled = false;
				}
				m_Resetter = m_SpawnedEffect.GetComponent<PoolableEffect>();
				if (m_Resetter != null)
				{
					m_Resetter.TurnOnAllSystems();
				}
			}
		}

		/// <summary>
		/// Make effect follow us
		/// </summary>
		protected virtual void Update()
		{
			// Make the effect follow our position.
			// We don't reparent it because it should not be disabled when we are
			if (m_SpawnedEffect != null)
			{
				m_SpawnedEffect.transform.position = followTransform.position;
			}
		}

		/// <summary>
		/// Destroy and start destruction of effect
		/// </summary>
		protected virtual void OnDisable()
		{
			if (m_SpawnedEffect == null)
			{
				return;
			}
			
			// Initiate destruction timer
			if (m_DestroyTimer != null)
			{
				m_DestroyTimer.enabled = true;

				if (m_Resetter != null)
				{
					m_Resetter.StopAll();
				}
			}
			else
			{
				// Repool immediately
				Poolable.TryPool(m_SpawnedEffect);
			}

			m_SpawnedEffect = null;
			m_DestroyTimer = null;
		}
	}
}