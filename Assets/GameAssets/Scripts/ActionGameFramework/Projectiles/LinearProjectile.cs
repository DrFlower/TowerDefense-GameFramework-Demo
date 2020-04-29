using System;
using ActionGameFramework.Helpers;
using UnityEngine;

namespace ActionGameFramework.Projectiles
{
	/// <summary>
	/// Simple IProjectile implementation for a projectile that flies in a straight line, optionally under m_Acceleration.
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class LinearProjectile : MonoBehaviour, IProjectile
	{
		public float acceleration;

		public float startSpeed;

		protected bool m_Fired;

		protected Rigidbody m_Rigidbody;

		public event Action fired;

		/// <summary>
		/// Fires this projectile from a designated start point to a designated world coordinate.
		/// </summary>
		/// <param name="startPoint">Start point of the flight.</param>
		/// <param name="targetPoint">Target point to fly to.</param>
		public virtual void FireAtPoint(Vector3 startPoint, Vector3 targetPoint)
		{
			transform.position = startPoint;

			Fire(Ballistics.CalculateLinearFireVector(startPoint, targetPoint, startSpeed));
		}

		/// <summary>
		/// Fires this projectile in a designated direction.
		/// </summary>
		/// <param name="startPoint">Start point of the flight.</param>
		/// <param name="fireVector">Vector representing direction of flight.</param>
		public virtual void FireInDirection(Vector3 startPoint, Vector3 fireVector)
		{
			transform.position = startPoint;

			// If we have no initial speed, we provide a small one to give the launch vector a baseline magnitude.
			if (Math.Abs(startSpeed) < float.Epsilon)
			{
				startSpeed = 0.001f;
			}

			Fire(fireVector.normalized * startSpeed);
		}

		/// <summary>
		/// Fires this projectile at a designated starting velocity, overriding any starting speeds.
		/// </summary>
		/// <param name="startPoint">Start point of the flight.</param>
		/// <param name="fireVelocity">Vector3 representing launch velocity.</param>
		public void FireAtVelocity(Vector3 startPoint, Vector3 fireVelocity)
		{
			transform.position = startPoint;

			startSpeed = fireVelocity.magnitude;

			Fire(fireVelocity);
		}

		protected virtual void Awake()
		{
			m_Rigidbody = GetComponent<Rigidbody>();
		}

		protected virtual void Update()
		{
			if (!m_Fired)
			{
				return;
			}

			if (Math.Abs(acceleration) >= float.Epsilon)
			{
				m_Rigidbody.velocity += transform.forward * acceleration * Time.deltaTime;
			}
		}

		protected virtual void Fire(Vector3 firingVector)
		{
			m_Fired = true;

			transform.rotation = Quaternion.LookRotation(firingVector);

			m_Rigidbody.velocity = firingVector;

			if (fired != null)
			{
				fired();
			}
		}
	}
}