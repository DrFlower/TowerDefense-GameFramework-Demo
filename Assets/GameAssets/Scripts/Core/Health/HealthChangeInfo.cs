using UnityEngine;

namespace Core.Health
{
	/// <summary>
	/// Health change info - stores information about the health change
	/// </summary>
	public struct HealthChangeInfo
	{
		public Damageable damageable;

		public float oldHealth;

		public float newHealth;

		public IAlignmentProvider damageAlignment;

		public float healthDifference
		{
			get { return newHealth - oldHealth; }
		}

		public float absHealthDifference
		{
			get { return Mathf.Abs(healthDifference); }
		}
	}
}