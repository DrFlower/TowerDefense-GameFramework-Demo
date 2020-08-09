using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public class EntityProjectileWobblingHoming : EntityHideSelfProjectile
    {
        public float acceleration;

        public float startSpeed;

        protected bool m_Fired;

        protected Rigidbody m_Rigidbody;

        public int leadingPrecision = 2;

        public bool leadTarget;

        protected EntityTargetable enemy;

        static readonly Collider[] s_Enemies = new Collider[64];
        public LayerMask mask = -1;

        Vector3 m_TargetVelocity;

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

        /// <summary>
        /// Angle, in degrees, to rotate, on the x axis, the fire vector by
        /// </summary>
        public float fireVectorXRotationAdjustment = 45.0f;

        private Vector3 tempVelocity;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Rigidbody = GetComponent<Rigidbody>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            enemy = entityDataProjectile.EntityTargetable;

            Vector3 startingPoint = entityDataProjectile.FiringPoint.position;
            Vector3 targetPoint = Ballistics.CalculateLinearLeadingTargetPoint(
                startingPoint, enemy.transform.position,
                enemy.Velocity, startSpeed,
                acceleration);


            Vector3 direction = entityDataProjectile.FiringPoint.forward;

            Vector3 binormal = Vector3.Cross(direction, Vector3.up);
            Quaternion rotation = Quaternion.AngleAxis(fireVectorXRotationAdjustment, binormal);

            Vector3 adjustedFireVector = rotation * direction;

            FireInDirection(startingPoint, adjustedFireVector);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (enemy == null || m_State == State.Targeting)
            {
                if (!m_Fired)
                {
                    return;
                }

                if (enemy == null)
                {
                    m_Rigidbody.rotation = Quaternion.LookRotation(m_Rigidbody.velocity);
                    return;
                }

                Quaternion aimDirection = Quaternion.LookRotation(GetHeading());

                m_Rigidbody.rotation = aimDirection;
                m_Rigidbody.velocity = transform.forward * m_Rigidbody.velocity.magnitude;

                if (!m_Fired)
                {
                    return;
                }

                if (Math.Abs(acceleration) >= float.Epsilon)
                {
                    m_Rigidbody.velocity += transform.forward * acceleration * Time.deltaTime;
                }
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
                        m_TargetWobbleVector = new Vector3(UnityEngine.Random.Range(-wobbleMagnitude, wobbleMagnitude),
                                                           UnityEngine.Random.Range(-wobbleMagnitude, wobbleMagnitude), 0);
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

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            tempVelocity = Vector3.zero;
        }

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

        private void FixedUpdate()
        {
            if (enemy == null)
            {
                return;
            }

            m_TargetVelocity = enemy.Velocity;
        }

        protected Vector3 GetHeading()
        {
            if (enemy == null)
            {
                return Vector3.zero;
            }
            Vector3 heading;
            if (leadTarget)
            {
                heading = Ballistics.CalculateLinearLeadingTargetPoint(transform.position, enemy.transform.position,
                                                                       m_TargetVelocity, m_Rigidbody.velocity.magnitude,
                                                                       acceleration,
                                                                       leadingPrecision) - transform.position;
            }
            else
            {
                heading = enemy.transform.position - transform.position;
            }

            return heading.normalized;
        }

        protected void Fire(Vector3 firingVector)
        {
            m_TargetWobbleVector = new Vector3(UnityEngine.Random.Range(-wobbleMagnitude, wobbleMagnitude),
                                   UnityEngine.Random.Range(-wobbleMagnitude, wobbleMagnitude), 0);
            m_WobbleDuration = UnityEngine.Random.Range(wobbleTimeRange.x, wobbleTimeRange.y);

            if (enemy == null)
            {
                Debug.LogError("Homing target has not been specified. Aborting fire.");
                return;
            }
            enemy.OnHidden += OnTargetLost;
            enemy.OnDead += OnTargetLost;

            m_Fired = true;
            transform.rotation = Quaternion.LookRotation(firingVector);
            m_Rigidbody.velocity = firingVector;

            m_State = State.Wobbling;
            m_CurrentWobbleTime = 0.0f;
        }

        void OnTargetLost(EntityTargetable enemy)
        {
            enemy.OnHidden -= OnTargetLost;
            enemy.OnDead -= OnTargetLost;
            this.enemy = null;
        }

        void OnTriggerEnter(Collider other)
        {
            EntityEnemy enemy = other.gameObject.GetComponent<EntityEnemy>();
            if (enemy == null)
                return;

            if (!enemy.IsDead)
                enemy.Damage(entityDataProjectile.ProjectileData.Damage);

            int number = Physics.OverlapSphereNonAlloc(transform.position, entityDataProjectile.ProjectileData.SplashRange, s_Enemies, mask);
            for (int index = 0; index < number; index++)
            {
                Collider collider = s_Enemies[index];
                var rangeEnemy = collider.GetComponent<EntityEnemy>();
                if (rangeEnemy == null)
                {
                    continue;
                }
                if (!enemy.IsDead)
                    rangeEnemy.Damage(entityDataProjectile.ProjectileData.SplashDamage);
            }

            SpawnCollisionParticles();

            if (!hide)
            {
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Entity.Id));
                hide = true;
            }
        }

        public override void Pause()
        {
            base.Pause();
            tempVelocity = m_Rigidbody.velocity;
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.isKinematic = true;
            m_Rigidbody.isKinematic = false;
        }

        public override void Resume()
        {
            base.Resume();
            m_Rigidbody.velocity = tempVelocity;
        }
    }
}
