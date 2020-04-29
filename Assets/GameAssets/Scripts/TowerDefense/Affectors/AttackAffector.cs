using System.Collections.Generic;
using ActionGameFramework.Audio;
using ActionGameFramework.Health;
using Core.Health;
using TowerDefense.Targetting;
using TowerDefense.Towers;
using TowerDefense.Towers.Projectiles;
using UnityEngine;

namespace TowerDefense.Affectors
{
	/// <summary>
	/// The common effect for handling firing projectiles to attack
	/// 
	/// Requires an ILauncher but it is not automatically added
	/// Add an ILauncher implementation to this GameObject before you add this script
	/// </summary>
	[RequireComponent(typeof(ILauncher))]
	public class AttackAffector : Affector, ITowerRadiusProvider
	{
		/// <summary>
		/// The projectile used to attack
		/// </summary>
		public GameObject projectile;

		/// <summary>
		/// The list of points to launch the projectiles from
		/// </summary>
		public Transform[] projectilePoints;

		/// <summary>
		/// The reference to the center point where the tower will search from
		/// </summary>
		public Transform epicenter;

		/// <summary>
		/// Configuration for when the tower does splash damage
		/// </summary>
		public bool isMultiAttack;


		/// <summary>
		/// The fire rate in fires-per-second
		/// </summary>
		public float fireRate;

		/// <summary>
		/// The audio source to play when firing
		/// </summary>
		public RandomAudioSource randomAudioSource;

		/// <summary>
		/// Gets the targetter
		/// </summary>
		public Targetter towerTargetter;

		/// <summary>
		/// Color of effect radius visualization
		/// </summary>
		public Color radiusEffectColor;

		/// <summary>
		/// Search condition
		/// </summary>
		public Filter searchCondition;

		/// <summary>
		/// Fire condition
		/// </summary>
		public Filter fireCondition;

		/// <summary>
		/// The reference to the attached launcher
		/// </summary>
		protected ILauncher m_Launcher;

		/// <summary>
		/// The time before firing is possible
		/// </summary>
		protected float m_FireTimer;

		/// <summary>
		/// Reference to the current tracked enemy
		/// </summary>
		protected Targetable m_TrackingEnemy;

		/// <summary>
		/// Gets the search rate from the targetter
		/// </summary>
		public float searchRate
		{
			get { return towerTargetter.searchRate; }
			set { towerTargetter.searchRate = value; }
		}

		/// <summary>
		/// Gets the targetable
		/// </summary>
		public Targetable trackingEnemy
		{
			get { return m_TrackingEnemy; }
		}

		/// <summary>
		/// Gets or sets the attack radius
		/// </summary>
		public float effectRadius
		{
			get { return towerTargetter.effectRadius; }
		}

		public Color effectColor 
		{
			get { return radiusEffectColor; }
		}

		public Targetter targetter 
		{
			get { return towerTargetter; }
		}

		/// <summary>
		/// Initializes the attack affector
		/// </summary>
		public override void Initialize(IAlignmentProvider affectorAlignment)
		{
			Initialize(affectorAlignment, -1);
		}

		/// <summary>
		/// Initialises the  attack affector with a layer mask
		/// </summary>
		public override void Initialize(IAlignmentProvider affectorAlignment, LayerMask mask)
		{
			base.Initialize(affectorAlignment, mask);
			SetUpTimers();

			towerTargetter.ResetTargetter();
			towerTargetter.alignment = affectorAlignment;
			towerTargetter.acquiredTarget += OnAcquiredTarget;
			towerTargetter.lostTarget += OnLostTarget;
		}

		void OnDestroy()
		{
			towerTargetter.acquiredTarget -= OnAcquiredTarget;
			towerTargetter.lostTarget -= OnLostTarget;
		}

		void OnLostTarget()
		{
			m_TrackingEnemy = null;
		}

		void OnAcquiredTarget(Targetable acquiredTarget)
		{
			m_TrackingEnemy = acquiredTarget;
		}

		public Damager damagerProjectile
		{
			get { return projectile == null ? null : projectile.GetComponent<Damager>(); }
		}

		/// <summary>
		/// Returns the total projectile damage 
		/// </summary>
		public float GetProjectileDamage()
		{
			var splash = projectile.GetComponent<SplashDamager>();
			float splashDamage = splash != null ? splash.damage : 0;
			return damagerProjectile.damage + splashDamage;
		}

		/// <summary>
		/// Initialise the RepeatingTimer
		/// </summary>
		protected virtual void SetUpTimers()
		{
			m_FireTimer = 1 / fireRate;
			m_Launcher = GetComponent<ILauncher>();
		}

		/// <summary>
		/// Update the timers
		/// </summary>
		protected virtual void Update()
		{
			m_FireTimer -= Time.deltaTime;
			if (trackingEnemy != null && m_FireTimer <= 0.0f)
			{
				OnFireTimer();
				m_FireTimer = 1 / fireRate;
			}
		}

		/// <summary>
		/// Fired at every poll of the fire rate timer
		/// </summary>
		protected virtual void OnFireTimer()
		{
			if (fireCondition != null)
			{
				if (!fireCondition())
				{
					return;
				}
			}
			FireProjectile();
		}

		/// <summary>
		/// Common logic when attacking
		/// </summary>
		protected virtual void FireProjectile()
		{
			if (m_TrackingEnemy == null)
			{
				return;
			}

			if (isMultiAttack)
			{
				List<Targetable> enemies = towerTargetter.GetAllTargets();
				m_Launcher.Launch(enemies, projectile, projectilePoints);
			}
			else
			{
				m_Launcher.Launch(m_TrackingEnemy, damagerProjectile.gameObject, projectilePoints);
			}
			if (randomAudioSource != null)
			{
				randomAudioSource.PlayRandomClip();
			}
		}

		/// <summary>
		/// A delegate to compare distances of components
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		protected virtual int ByDistance(Targetable first, Targetable second)
		{
			float firstSqrMagnitude = Vector3.SqrMagnitude(first.position - epicenter.position);
			float secondSqrMagnitude = Vector3.SqrMagnitude(second.position - epicenter.position);
			return firstSqrMagnitude.CompareTo(secondSqrMagnitude);
		}

#if UNITY_EDITOR
		/// <summary>
		/// Draws the search area
		/// </summary>
		void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(epicenter.position, towerTargetter.effectRadius);
		}
#endif
	}

	/// <summary>
	/// A delegate for boolean calculation logic
	/// </summary>
	public delegate bool Filter();
}