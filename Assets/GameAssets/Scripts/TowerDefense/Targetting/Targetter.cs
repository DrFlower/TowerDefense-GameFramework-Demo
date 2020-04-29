using System;
using System.Collections.Generic;
using ActionGameFramework.Health;
using Core.Health;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefense.Targetting
{
	/// <summary>
	/// Class used to track targets for an affector
	/// </summary>
	public class Targetter : MonoBehaviour
	{
		/// <summary>
		/// Fires when a targetable enters the target collider
		/// </summary>
		public event Action<Targetable> targetEntersRange;

		/// <summary>
		/// Fires when a targetable exits the target collider
		/// </summary>
		public event Action<Targetable> targetExitsRange;

		/// <summary>
		/// Fires when an appropriate target is found
		/// </summary>
		public event Action<Targetable> acquiredTarget;

		/// <summary>
		/// Fires when the current target was lost
		/// </summary>
		public event Action lostTarget;

		/// <summary>
		/// The transform to point at the target
		/// </summary>
		public Transform turret;

		/// <summary>
		/// The range of the turret's x rotation
		/// </summary>
		public Vector2 turretXRotationRange = new Vector2(0, 359);

		/// <summary>
		/// If m_Turret rotates freely or only on y;
		/// </summary>
		public bool onlyYTurretRotation;

		/// <summary>
		/// The search rate in searches per second
		/// </summary>
		public float searchRate;

		/// <summary>
		/// Y rotation speed while the turret is idle in degrees per second
		/// </summary>
		public float idleRotationSpeed = 39f;

		/// <summary>
		/// The time it takes for the tower to correct its x rotation on idle in seconds
		/// </summary>
		public float idleCorrectionTime = 2.0f;

		/// <summary>
		/// The collider attached to the targetter
		/// </summary>
		public Collider attachedCollider;

		/// <summary>
		/// How long the turret waits in its idle form before spinning in seconds
		/// </summary>
		public float idleWaitTime = 2.0f;

		/// <summary>
		/// The current targetables in the collider
		/// </summary>
		protected List<Targetable> m_TargetsInRange = new List<Targetable>();

		/// <summary>
		/// The seconds until a search is allowed
		/// </summary>
		protected float m_SearchTimer = 0.0f;

		/// <summary>
		/// The seconds until the tower starts spinning
		/// </summary>
		protected float m_WaitTimer = 0.0f;

		/// <summary>
		/// The current targetable
		/// </summary>
		protected Targetable m_CurrrentTargetable;

		/// <summary>
		/// Counter used for x rotation correction
		/// </summary>
		protected float m_XRotationCorrectionTime;

		/// <summary>
		/// If there was a targetable in the last frame
		/// </summary>
		protected bool m_HadTarget;

		/// <summary>
		/// How fast this turret is spinning
		/// </summary>
		protected float m_CurrentRotationSpeed;

		/// <summary>
		/// returns the radius of the collider whether
		/// its a sphere or capsule
		/// </summary>
		public float effectRadius
		{
			get
			{
				var sphere = attachedCollider as SphereCollider;
				if (sphere != null)
				{
					return sphere.radius;
				}
				var capsule = attachedCollider as CapsuleCollider;
				if (capsule != null)
				{
					return capsule.radius;
				}
				return 0;
			}
		}

		/// <summary>
		/// The alignment of the affector
		/// </summary>
		public IAlignmentProvider alignment;

		/// <summary>
		/// Returns the current target
		/// </summary>
		public Targetable GetTarget()
		{
			return m_CurrrentTargetable;
		}

		/// <summary>
		/// Clears the list of current targets and clears all events
		/// </summary>
		public void ResetTargetter()
		{
			m_TargetsInRange.Clear();
			m_CurrrentTargetable = null;

			targetEntersRange = null;
			targetExitsRange = null;
			acquiredTarget = null;
			lostTarget = null;

			// Reset turret facing
			if (turret != null)
			{
				turret.localRotation = Quaternion.identity;
			}
		}

		/// <summary>
		/// Returns all the targets within the collider. This list must not be changed as it is the working
		/// list of the targetter. Changing it could break the targetter
		/// </summary>
		public List<Targetable> GetAllTargets()
		{
			return m_TargetsInRange;
		}

		/// <summary>
		/// Checks if the targetable is a valid target
		/// </summary>
		/// <param name="targetable"></param>
		/// <returns>true if targetable is vaild, false if not</returns>
		protected virtual bool IsTargetableValid(Targetable targetable)
		{
			if (targetable == null)
			{
				return false;
			}
			
			IAlignmentProvider targetAlignment = targetable.configuration.alignmentProvider;
			bool canDamage = alignment == null || targetAlignment == null ||
			                 alignment.CanHarm(targetAlignment);
			
			return canDamage;
		}

		/// <summary>
		/// On exiting the trigger, a valid targetable is removed from the tracking list.
		/// </summary>
		/// <param name="other">The other collider in the collision</param>
		protected virtual void OnTriggerExit(Collider other)
		{
			var targetable = other.GetComponent<Targetable>();
			if (!IsTargetableValid(targetable))
			{
				return;
			}
			
			m_TargetsInRange.Remove(targetable);
			if (targetExitsRange != null)
			{
				targetExitsRange(targetable);
			}
			if (targetable == m_CurrrentTargetable)
			{
				OnTargetRemoved(targetable);
			}
			else
			{
				// Only need to remove if we're not our actual target, otherwise OnTargetRemoved will do the work above
				targetable.removed -= OnTargetRemoved;
			}
		}
 
		/// <summary>
		/// On entering the trigger, a valid targetable is added to the tracking list.
		/// </summary>
		/// <param name="other">The other collider in the collision</param>
		protected virtual void OnTriggerEnter(Collider other)
		{
			var targetable = other.GetComponent<Targetable>();
			if (!IsTargetableValid(targetable))
			{
				return;
			}
			targetable.removed += OnTargetRemoved;
			m_TargetsInRange.Add(targetable);
			if (targetEntersRange != null)
			{
				targetEntersRange(targetable);
			}
		}

		/// <summary>
		/// Returns the nearest targetable within the currently tracked targetables 
		/// </summary>
		/// <returns>The nearest targetable if there is one, null otherwise</returns>
		protected virtual Targetable GetNearestTargetable()
		{
			int length = m_TargetsInRange.Count;

			if (length == 0)
			{
				return null;
			}

			Targetable nearest = null;
			float distance = float.MaxValue;
			for (int i = length - 1; i >= 0; i--)
			{
				Targetable targetable = m_TargetsInRange[i];
				if (targetable == null || targetable.isDead)
				{
					m_TargetsInRange.RemoveAt(i);
					continue;
				}
				float currentDistance = Vector3.Distance(transform.position, targetable.position);
				if (currentDistance < distance)
				{
					distance = currentDistance;
					nearest = targetable;
				}
			}

			return nearest;
		}

		/// <summary>
		/// Starts the search timer
		/// </summary>
		protected virtual void Start()
		{
			m_SearchTimer = searchRate;
			m_WaitTimer = idleWaitTime;
		}

		/// <summary>
		/// Checks if any targets are destroyed and aquires a new targetable if appropriate
		/// </summary>
		protected virtual void Update()
		{
			m_SearchTimer -= Time.deltaTime;

			if (m_SearchTimer <= 0.0f && m_CurrrentTargetable == null && m_TargetsInRange.Count > 0)
			{
				m_CurrrentTargetable = GetNearestTargetable();
				if (m_CurrrentTargetable != null)
				{
					if (acquiredTarget != null)
					{
						acquiredTarget(m_CurrrentTargetable);
					}
					m_SearchTimer = searchRate;
				}
			}

			AimTurret();

			m_HadTarget = m_CurrrentTargetable != null;
		}

		/// <summary>
		/// Fired by the agents died event or when the current target moves out of range,
		/// Fires the lostTarget event.
		/// </summary>
		void OnTargetRemoved(DamageableBehaviour target)
		{
			target.removed -= OnTargetRemoved;
			if (m_CurrrentTargetable != null && target.configuration == m_CurrrentTargetable.configuration)
			{
				if (lostTarget != null)
				{
					lostTarget();
				}
				m_HadTarget = false;
				m_TargetsInRange.Remove(m_CurrrentTargetable);
				m_CurrrentTargetable = null;
				m_XRotationCorrectionTime = 0.0f;
			}
			else //wasnt the current target, find and remove from targets list
			{
				for (int i = 0; i < m_TargetsInRange.Count; i++)
				{
					if (m_TargetsInRange[i].configuration == target.configuration)
					{
						m_TargetsInRange.RemoveAt(i);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Aims the turret at the current target
		/// </summary>
		protected virtual void AimTurret()
		{
			if (turret == null)
			{
				return;
			}

			if (m_CurrrentTargetable == null) // do idle rotation
			{
				if (m_WaitTimer > 0)
				{
					m_WaitTimer -= Time.deltaTime;
					if (m_WaitTimer <= 0)
					{
						m_CurrentRotationSpeed = (Random.value * 2 - 1) * idleRotationSpeed;
					}
				}
				else
				{
					Vector3 euler = turret.rotation.eulerAngles;
					euler.x = Mathf.Lerp(Wrap180(euler.x), 0, m_XRotationCorrectionTime);
					m_XRotationCorrectionTime = Mathf.Clamp01((m_XRotationCorrectionTime + Time.deltaTime) / idleCorrectionTime);
					euler.y += m_CurrentRotationSpeed * Time.deltaTime;

					turret.eulerAngles = euler;
				}
			}
			else
			{
				m_WaitTimer = idleWaitTime;

				Vector3 targetPosition = m_CurrrentTargetable.position;
				if (onlyYTurretRotation)
				{
					targetPosition.y = turret.position.y;
				}
				Vector3 direction = targetPosition - turret.position;
				Quaternion look = Quaternion.LookRotation(direction, Vector3.up);
				Vector3 lookEuler = look.eulerAngles;
				// We need to convert the rotation to a -180/180 wrap so that we can clamp the angle with a min/max
				float x = Wrap180(lookEuler.x);
				lookEuler.x = Mathf.Clamp(x, turretXRotationRange.x, turretXRotationRange.y);
				look.eulerAngles = lookEuler;
				turret.rotation = look;
			}
		}

		/// <summary>
		/// A simply function to convert an angle to a -180/180 wrap
		/// </summary>
		static float Wrap180(float angle)
		{
			angle %= 360;
			if (angle < -180)
			{
				angle += 360;
			}
			else if (angle > 180)
			{
				angle -= 360;
			}
			return angle;
		}
	}
}