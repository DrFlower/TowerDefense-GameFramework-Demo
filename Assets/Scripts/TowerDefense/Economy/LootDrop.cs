using Core.Health;
using TowerDefense.Level;
using UnityEngine;

namespace TowerDefense.Economy
{
	/// <summary>
	/// A class that adds money to the currency when the attached DamagableBehaviour dies
	/// </summary>
	[RequireComponent(typeof(DamageableBehaviour))]
	public class LootDrop : MonoBehaviour
	{
		/// <summary>
		/// The amount of loot/currency dropped when object "dies"
		/// </summary>
		public int lootDropped = 1;

		/// <summary>
		/// The attached DamagableBehaviour
		/// </summary>
		protected DamageableBehaviour m_DamageableBehaviour;

		/// <summary>
		/// Caches attached DamageableBehaviour
		/// </summary>
		protected virtual void OnEnable()
		{
			if (m_DamageableBehaviour == null)
			{
				m_DamageableBehaviour = GetComponent<DamageableBehaviour>();
			}
			m_DamageableBehaviour.configuration.died += OnDeath;
		}

		/// <summary>
		/// Unsubscribed from the <see cref="m_DamageableBehaviour"/> died event
		/// </summary>
		protected virtual void OnDisable()
		{
			m_DamageableBehaviour.configuration.died -= OnDeath;
		}

		/// <summary>
		/// The callback for when the attached object "dies".
		/// Add <see cref="lootDropped"/> to current currency
		/// </summary>
		protected virtual void OnDeath(HealthChangeInfo info)
		{
			m_DamageableBehaviour.configuration.died -= OnDeath;

			if (info.damageAlignment == null ||
				!info.damageAlignment.CanHarm(m_DamageableBehaviour.configuration.alignmentProvider))
			{
				return;
			}
			
			LevelManager levelManager = LevelManager.instance;
			if (levelManager == null)
			{
				return;
			}
			levelManager.currency.AddCurrency(lootDropped);
		}
	}
}