using System.Collections.Generic;
using ActionGameFramework.Health;
using Core.Utilities;
using UnityEngine;

namespace TowerDefense.Towers.TowerLaunchers
{
	public abstract class Launcher : MonoBehaviour, ILauncher
	{
		public abstract void Launch(Targetable enemy, GameObject attack, Transform firingPoint);

		/// <summary>
		/// Gets an instance of the attack object from the Pool and Launches it
		/// </summary>
		/// <param name="enemies">
		/// The list of enemies to sample from
		/// </param>
		/// <param name="attack">
		/// The object used to attack
		/// </param>
		/// <param name="firingPoints"></param>
		public virtual void Launch(List<Targetable> enemies, GameObject attack, Transform[] firingPoints)
		{
			int count = enemies.Count;
			int currentFiringPointIndex = 0;
			int firingPointLength = firingPoints.Length;
			for (int i = 0; i < count; i++)
			{
				Targetable enemy = enemies[i];
				Transform firingPoint = firingPoints[currentFiringPointIndex];
				currentFiringPointIndex = (currentFiringPointIndex + 1) % firingPointLength;
				var poolable = Poolable.TryGetPoolable<Poolable>(attack);
				if (poolable == null)
				{
					return;
				}
				Launch(enemy, poolable.gameObject, firingPoint);
			}
		}

		/// <summary>
		/// Gets a instance of attack from the Pool and Launches it
		/// </summary>
		/// <param name="enemy">
		/// The enemy launcher is attacking
		/// </param>
		/// <param name="attack">
		/// The object used to attack the enemy
		/// </param>
		/// <param name="firingPoints"></param>
		public virtual void Launch(Targetable enemy, GameObject attack, Transform[] firingPoints)
		{
			var poolable = Poolable.TryGetPoolable<Poolable>(attack);
			if (poolable == null)
			{
				return;
			}
			Launch(enemy, poolable.gameObject, GetRandomTransform(firingPoints));
		}

		/// <summary>
		/// Sets up a particle system to provide aiming feedback
		/// </summary>
		/// <param name="particleSystemToPlay">
		/// The Particle system to fire
		/// </param>
		/// <param name="origin">
		/// The position of the particle system
		/// </param>
		/// <param name="lookPosition">
		/// The direction the particle system is looking
		/// </param>
		public void PlayParticles(ParticleSystem particleSystemToPlay, Vector3 origin, Vector3 lookPosition)
		{
			if (particleSystemToPlay == null)
			{
				return;
			}
			particleSystemToPlay.transform.position = origin;
			particleSystemToPlay.transform.LookAt(lookPosition);
			particleSystemToPlay.Play();
		}

		/// <summary>
		/// Gets a random transform from a list
		/// </summary>
		/// <param name="launchPoints">
		/// The list of transforms to use
		/// </param>
		public Transform GetRandomTransform(Transform[] launchPoints)
		{
			int index = Random.Range(0, launchPoints.Length);
			return launchPoints[index];
		}
	}
}