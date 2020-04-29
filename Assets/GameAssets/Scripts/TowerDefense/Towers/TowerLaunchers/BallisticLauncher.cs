using ActionGameFramework.Health;
using ActionGameFramework.Helpers;
using ActionGameFramework.Projectiles;
using TowerDefense.Level;
using UnityEngine;

namespace TowerDefense.Towers.TowerLaunchers
{
	/// <summary>
	/// Implementation of the tower launcher for Ballistic Projectiles
	/// </summary>
	public class BallisticLauncher : Launcher
	{
		/// <summary>
		/// The particle system used for providing launch feedback
		/// </summary>
		public ParticleSystem fireParticleSystem;

		/// <summary>
		/// Launches a single projectile at a single enemy from a single firing point
		/// </summary>
		/// <param name="enemy">
		/// The enemy to target
		/// </param>
		/// <param name="projectile">
		/// The projectile to attack
		/// </param>
		/// <param name="firingPoint">
		/// The point to fire from
		/// </param>
		public override void Launch(Targetable enemy, GameObject projectile, Transform firingPoint)
		{
			Vector3 startPosition = firingPoint.position;
			var ballisticProjectile = projectile.GetComponent<BallisticProjectile>();
			if (ballisticProjectile == null)
			{
				Debug.LogError("No ballistic projectile attached to projectile");
				DestroyImmediate(projectile);
				return;
			}
			Vector3 targetPoint;
			if (ballisticProjectile.fireMode == BallisticFireMode.UseLaunchSpeed)
			{
				// use speed
				targetPoint = Ballistics.CalculateBallisticLeadingTargetPointWithSpeed(
					startPosition,
					enemy.position, enemy.velocity,
					ballisticProjectile.startSpeed, ballisticProjectile.arcPreference, Physics.gravity.y, 4);
			}
			else
			{
				// use angle
				targetPoint = Ballistics.CalculateBallisticLeadingTargetPointWithAngle(
					startPosition,
					enemy.position, enemy.velocity, ballisticProjectile.firingAngle,
					ballisticProjectile.arcPreference, Physics.gravity.y, 4);
			}
			ballisticProjectile.FireAtPoint(startPosition, targetPoint);
			ballisticProjectile.IgnoreCollision(LevelManager.instance.environmentColliders);
			PlayParticles(fireParticleSystem, startPosition, targetPoint);
		}
	}
}