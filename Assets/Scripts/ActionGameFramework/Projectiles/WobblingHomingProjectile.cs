using UnityEngine;

namespace ActionGameFramework.Projectiles
{
	/// <summary>
	/// A projectile that shoots upwards and then starts homing
	/// </summary>
	public class WobblingHomingProjectile : HomingLinearProjectile
	{
		protected enum State
		{
			Wobbling,
			Turning,
			Targeting
		}

		/// <summary>
		/// The time the projectile wobbles upward is randomized from this range
		/// </summary>
		public Vector2 wobbleTimeRange = new Vector2(1, 2);

		/// <summary>
		/// The number of wobble direction changes per second
		/// </summary>
		public float wobbleDirectionChangeSpeed = 4;

		/// <summary>
		/// The intensity of the wobble
		/// </summary>
		public float wobbleMagnitude = 7;

		/// <summary>
		/// The time the projectile takes to turn and home
		/// </summary>
		public float turningTime = 0.5f;

		/// <summary>
		/// State of projectile
		/// </summary>
		State m_State;

		/// <summary>
		/// Seconds wobbling
		/// </summary>
		protected float m_CurrentWobbleTime;

		/// <summary>
		/// Total time to wobble
		/// </summary>
		protected float m_WobbleDuration;

		/// <summary>
		/// Seconds turning to face homing target
		/// </summary>
		protected float m_CurrentTurnTime;

		/// <summary>
		/// Seconds for current turn
		/// </summary>
		protected float m_WobbleChangeTime;

		protected Vector3 m_WobbleVector,
		                  m_TargetWobbleVector;

		protected override void Update()
		{
			// regular HomingLinearProjectile behaviour, handles a null homing target
			if (m_HomingTarget == null || m_State == State.Targeting)
			{
				base.Update();
				return;
			}
			switch (m_State)
			{
				// wobble the projectile
				case State.Wobbling:
					m_CurrentWobbleTime += Time.deltaTime;
					if (m_CurrentWobbleTime >= m_WobbleDuration)
					{
						m_State = State.Turning;
						m_CurrentTurnTime = 0;
					}

					m_WobbleChangeTime += Time.deltaTime * wobbleDirectionChangeSpeed;
					if (m_WobbleChangeTime >= 1)
					{
						m_WobbleChangeTime = 0;
						m_TargetWobbleVector = new Vector3(Random.Range(-wobbleMagnitude, wobbleMagnitude),
						                                   Random.Range(-wobbleMagnitude, wobbleMagnitude), 0);
						m_WobbleVector = Vector3.zero;
					}
					m_WobbleVector = Vector3.Lerp(m_WobbleVector, m_TargetWobbleVector, m_WobbleChangeTime);
					m_Rigidbody.velocity = Quaternion.Euler(m_WobbleVector) * m_Rigidbody.velocity;

					m_Rigidbody.rotation = Quaternion.LookRotation(m_Rigidbody.velocity);
					break;
				// turn the projectile to face the homing target
				case State.Turning:
					m_CurrentTurnTime += Time.deltaTime;
					Quaternion aimDirection = Quaternion.LookRotation(GetHeading());

					m_Rigidbody.rotation = Quaternion.Lerp(m_Rigidbody.rotation, aimDirection, m_CurrentTurnTime / turningTime);
					m_Rigidbody.velocity = transform.forward * m_Rigidbody.velocity.magnitude;

					if (m_CurrentTurnTime >= turningTime)
					{
						m_State = State.Targeting;
					}
					break;
			}
		}

		// select first wobble vector and set to wobble state
		protected override void Fire(Vector3 firingVector)
		{
			m_TargetWobbleVector = new Vector3(Random.Range(-wobbleMagnitude, wobbleMagnitude),
			                                   Random.Range(-wobbleMagnitude, wobbleMagnitude), 0);
			m_WobbleDuration = Random.Range(wobbleTimeRange.x, wobbleTimeRange.y);
			base.Fire(firingVector);
			m_State = State.Wobbling;
			m_CurrentWobbleTime = 0.0f;
		}
	}
}