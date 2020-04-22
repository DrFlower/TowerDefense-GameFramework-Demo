using ActionGameFramework.Health;
using TowerDefense.Towers.Projectiles;
using UnityEngine;

namespace TowerDefense.Towers.TowerLaunchers
{
	/// <summary>
	/// An implementation of the tower launcher for hitscan attacks
	/// </summary>
	public class HitscanLauncher : Launcher
	{
		/// <summary>
		/// The particle system used for providing launch feedback
		/// </summary>
		public ParticleSystem fireParticleSystem;


		/// <summary>
		/// Assigns the correct damage to the hitscan object and
		/// attacks the enemy immediately.
		/// Early return if there is not HitscanAttack.cs attached to the attact object
		/// </summary>
		/// <param name="enemy">
		/// The enemy this tower is targeting
		/// </param>
		/// <param name="attack">
		/// The attacking component used to damage the enemy
		/// </param>
		/// <param name="firingPoint"></param>
		public override void Launch(Targetable enemy, GameObject attack, Transform firingPoint)
		{
			var hitscanAttack = attack.GetComponent<HitscanAttack>();
			if (hitscanAttack == null)
			{
				return;
			}
			hitscanAttack.transform.position = firingPoint.position;
			hitscanAttack.AttackEnemy(firingPoint.position, enemy);
			PlayParticles(fireParticleSystem, firingPoint.position, enemy.position);
		}
	}
}