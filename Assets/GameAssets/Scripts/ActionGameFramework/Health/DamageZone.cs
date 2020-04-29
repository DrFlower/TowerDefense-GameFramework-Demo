using Core.Health;
using UnityEngine;

namespace ActionGameFramework.Health
{
	/// <summary>
	/// Damage zone - an area that receives damage and gives it to the damageable behaviour
	/// Abstract base class because the actual damage mechanic can be based on Collisions or Triggers or some other mechanism
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public abstract class DamageZone : MonoBehaviour
	{
		/// <summary>
		/// Allow the user to define the damageable behaviour that this zone corresponds to.
		/// This allows multiple damage zones to reference one damageable behaviour
		/// allowing locational damage. e.g. headshots.
		/// </summary>
		[Tooltip("If this is empty, DamageZone will try to use a DamageableBehaviour on the same object.")]
		public DamageableBehaviour damageableBehaviour;

		/// <summary>
		/// Allow scaling of the damage. This allows different zones to take different damage from same Damager. e.g. headshots
		/// </summary>
		public float damageScale = 1f;

		/// <summary>
		/// Scales the damage.
		/// </summary>
		/// <returns>The damage.</returns>
		/// <param name="damage">Damage.</param>
		protected float ScaleDamage(float damage)
		{
			return damageScale * damage;
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