using System.Collections.Generic;
using ActionGameFramework.Health;
using ActionGameFramework.Helpers;
using ActionGameFramework.Projectiles;
using Core.Utilities;
using TowerDefense.Affectors;
using TowerDefense.Level;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense.Towers.TowerLaunchers
{
	/// <summary>
	/// Rapid fire launcher, launchers a homing projectile
	/// </summary>
	public class SuperTowerLauncher : HomingLauncher
	{
		/// <summary>
		/// How long the tower will stay active
		/// </summary>
		public float towerLifeSpan = 10;

		/// <summary>
		/// Angle, in degrees, to rotate, on the x axis, the fire vector by
		/// </summary>
		public float fireVectorXRotationAdjustment = 45.0f;

		/// <summary>
		/// Fires when the max amount of projectiles has been reached
		/// </summary>
		public UnityEvent death;
		
		/// <summary>
		/// Timer to invoke a unity event when it elapses
		/// </summary>
		protected Timer m_LifeTimer;

		/// <summary>
		///	Finds a random enemy in a list and fires from a random point
		/// </summary>
		/// <param name="enemies">
		/// The list of enemies to sample from
		/// </param>
		/// <param name="attack">
		/// The object used to attack
		/// </param>
		/// <param name="firingPoints"></param>
		public override void Launch(List<Targetable> enemies, GameObject attack, Transform[] firingPoints)
		{
			var poolable = Poolable.TryGetPoolable<Poolable>(attack);
			if (poolable == null)
			{
				return;
			}
			Targetable enemy = enemies[Random.Range(0, enemies.Count)];
			Transform firingPoint = GetRandomTransform(firingPoints);
			Launch(enemy, poolable.gameObject, firingPoint);
		}

		public override void Launch(Targetable enemy, GameObject attack, Transform firingPoint)
		{
			var homingMissile = attack.GetComponent<HomingLinearProjectile>();
			if (homingMissile == null)
			{
				Debug.LogError("No HomingLinearProjectile attached to attack object");
				return;
			}
			Vector3 startingPoint = firingPoint.position;
			Vector3 targetPoint = Ballistics.CalculateLinearLeadingTargetPoint(
				startingPoint, enemy.position,
				enemy.velocity, homingMissile.startSpeed,
				homingMissile.acceleration);

			homingMissile.SetHomingTarget(enemy);

			var attackAffector = GetComponent<AttackAffector>();
			Vector3 direction = attackAffector.towerTargetter.turret.forward;

			Vector3 binormal = Vector3.Cross(direction, Vector3.up);
			Quaternion rotation = Quaternion.AngleAxis(fireVectorXRotationAdjustment, binormal);

			Vector3 adjustedFireVector = rotation * direction;

			homingMissile.FireInDirection(startingPoint, adjustedFireVector);

			PlayParticles(fireParticleSystem, startingPoint, targetPoint);
		}

		/// <summary>
		/// Subscribes to Level Manager onStateChanged
		/// If waves have already begun, then begin death timer
		/// </summary>
		protected virtual void OnEnable()
		{
			if (!LevelManager.instanceExists)
			{
				return;
			}
			LevelState currentState = LevelManager.instance.levelState;
			if (currentState == LevelState.SpawningEnemies || currentState == LevelState.AllEnemiesSpawned)
			{
				m_LifeTimer = new Timer(towerLifeSpan, OnLifeTimerElapsed);
			}
			else
			{
				LevelManager.instance.levelStateChanged += OnLevelStateChanged;
			}
		}

		/// <summary>
		/// Unsubscribe from Level Manager onStateChanged
		/// </summary>
		protected virtual void OnDisable()
		{
			if (LevelManager.instanceExists)
			{
				LevelManager.instance.levelStateChanged -= OnLevelStateChanged;
			}
		}

		/// <summary>
		/// Tick the timer
		/// </summary>
		protected void Update()
		{
			if (m_LifeTimer == null)
			{
				return;
			}
			m_LifeTimer.Tick(Time.deltaTime);
		}

		/// <summary>
		/// Invoke the UnityEvent once the timer elapses
		/// </summary>
		protected void OnLifeTimerElapsed()
		{
			death.Invoke();
		}

		/// <summary>
		/// Checks the current state, if within a valid state, start the death timer
		/// </summary>
		/// <param name="previousState">
		/// The previous state the the LevelManager was in
		/// </param>
		/// <param name="currentState">
		/// The current state the LevelManager was in
		/// </param>
		void OnLevelStateChanged(LevelState previousState, LevelState currentState)
		{
			if (currentState == LevelState.SpawningEnemies || currentState == LevelState.AllEnemiesSpawned)
			{
				m_LifeTimer = new Timer(towerLifeSpan, OnLifeTimerElapsed);
			}
		}
	}
}