using ActionGameFramework.Health;
using ActionGameFramework.Helpers;
using Core.Health;
using UnityEngine;

namespace ActionGameFramework.Projectiles
{
	/// <summary>
	/// Basic override of LinearProjectile that allows them to adjust their path in-flight to intercept a designated target.
	/// </summary>
	public class HomingLinearProjectile : LinearProjectile
	{
		public int leadingPrecision = 2;

		public bool leadTarget;

		protected Targetable m_HomingTarget;

		Vector3 m_TargetVelocity;

		/// <summary>
		/// Sets the target transform that will be homed in on once fired.
		/// </summary>
		/// <param name="target">Transform of the target to home in on.</param>
		public void SetHomingTarget(Targetable target)
		{
			m_HomingTarget = target;
		}

		protected virtual void FixedUpdate()
		{
			if (m_HomingTarget == null)
			{
				return;
			}

			m_TargetVelocity = m_HomingTarget.velocity;
		}

		protected override void Update()
		{
			if (!m_Fired)
			{
				return;
			}

			if (m_HomingTarget == null)
			{
				m_Rigidbody.rotation = Quaternion.LookRotation(m_Rigidbody.velocity);
				return;
			}

			Quaternion aimDirection = Quaternion.LookRotation(GetHeading());

			m_Rigidbody.rotation = aimDirection;
			m_Rigidbody.velocity = transform.forward * m_Rigidbody.velocity.magnitude;

			base.Update();
		}

		protected Vector3 GetHeading()
		{
			if (m_HomingTarget == null)
			{
				return Vector3.zero;
			}
			Vector3 heading;
			if (leadTarget)
			{
				heading = Ballistics.CalculateLinearLeadingTargetPoint(transform.position, m_HomingTarget.position,
				                                                       m_TargetVelocity, m_Rigidbody.velocity.magnitude,
				                                                       acceleration,
				                                                       leadingPrecision) - transform.position;
			}
			else
			{
				heading = m_HomingTarget.position - transform.position;
			}

			return heading.normalized;
		}

		protected override void Fire(Vector3 firingVector)
		{
			if (m_HomingTarget == null)
			{
				Debug.LogError("Homing target has not been specified. Aborting fire.");
				return;
			}
			m_HomingTarget.removed += OnTargetDied;

			base.Fire(firingVector);
		}

		void OnTargetDied(DamageableBehaviour targetable)
		{
			targetable.removed -= OnTargetDied;
			m_HomingTarget = null;
		}
	}
}