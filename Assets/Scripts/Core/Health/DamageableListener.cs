using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Health
{
	/// <summary>
	/// A UnityEvent that passes through the HealthChangeInfo
	/// </summary>
	[Serializable]
	public class HealthChangeEvent : UnityEvent<HealthChangeInfo>
	{
	}

	/// <summary>
	/// A UnityEvent that passes through the HitInfo
	/// </summary>
	[Serializable]
	public class HitEvent : UnityEvent<HitInfo>
	{
	}

	/// <summary>
	/// Damageable listener.
	/// </summary>
	public class DamageableListener : MonoBehaviour
	{
		// The damageable behaviour to listen to
		[Tooltip("Leave this empty if the DamageableBehaviour and DamageableListener are on the same component")]
		public DamageableBehaviour damageableBehaviour;

		// Events for health change (i.e. healing/damage) - to be configured in the editor
		public HealthChangeEvent damaged;
		
		public HealthChangeEvent healed;

		// Events for death and max health - to be configured in the editor
		public UnityEvent died;

		public UnityEvent reachedMaxHealth;

		// Event for when health is change
		public HealthChangeEvent healthChanged;
		
		// Event for hits
		[Header("The hit event is different from the damage event as it also contains hit position data")]
		public HitEvent hit;

		/// <summary>
		/// Lazy loads the DamageableBehaviour
		/// </summary>
		protected virtual void Awake()
		{
			LazyLoad();
		}

		/// <summary>
		/// Subscribes to events
		/// </summary>
		protected virtual void OnEnable()
		{
			damageableBehaviour.configuration.died += OnDeath;
			damageableBehaviour.configuration.reachedMaxHealth += OnReachedMaxHealth;
			damageableBehaviour.configuration.healed += OnHealed;
			damageableBehaviour.configuration.damaged += OnDamaged;
			damageableBehaviour.configuration.healthChanged += OnHealthChanged;
			damageableBehaviour.hit += OnHit;
		}

		/// <summary>
		/// Unsubscribes from events on disable
		/// </summary>
		protected virtual void OnDisable()
		{
			damageableBehaviour.configuration.died -= OnDeath;
			damageableBehaviour.configuration.reachedMaxHealth -= OnReachedMaxHealth;
			damageableBehaviour.configuration.healed -= OnHealed;
			damageableBehaviour.configuration.damaged -= OnDamaged;
			damageableBehaviour.configuration.healthChanged -= OnHealthChanged;
			damageableBehaviour.hit -= OnHit;
		}

		/// <summary>
		/// Raises the death UnityEvent.
		/// </summary>
		protected virtual void OnDeath(HealthChangeInfo info)
		{
			died.Invoke();
		}

		/// <summary>
		/// Raises the max health UnityEvent.
		/// </summary>
		protected virtual void OnReachedMaxHealth()
		{
			reachedMaxHealth.Invoke();
		}

		/// <summary>
		/// Raises the heal UnityEvent.
		/// </summary>
		/// <param name="info">Info.</param>
		protected virtual void OnHealed(HealthChangeInfo info)
		{
			healed.Invoke(info);
		}

		/// <summary>
		/// Raises the damage UnityEvent.
		/// </summary>
		/// <param name="info">Info.</param>
		protected virtual void OnDamaged(HealthChangeInfo info)
		{
			damaged.Invoke(info);
		}
		
		/// <summary>
		/// Raises the healthChanged UnityEvent.
		/// </summary>
		/// <param name="info">Info.</param>
		protected virtual void OnHealthChanged(HealthChangeInfo info)
		{
			healthChanged.Invoke(info);
		}

		/// <summary>
		/// Raises the hit UnityEvent.
		/// </summary>
		/// <param name="info">Info.</param>
		protected virtual void OnHit(HitInfo info)
		{
			hit.Invoke(info);
		}

		/// <summary>
		/// Looks for the damageableBehaviour if it is not already assigned
		/// It may be assigned in editor or from a previous LazyLoad() call
		/// </summary>
		protected void LazyLoad()
		{
			if (damageableBehaviour != null)
			{
				return;
			}

			damageableBehaviour = GetComponent<DamageableBehaviour>();
		}
	}
}