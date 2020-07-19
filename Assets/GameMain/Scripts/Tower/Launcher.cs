using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Flower
{
    public abstract class Launcher : MonoBehaviour, ILauncher
    {
        public abstract void Launch(EntityBaseEnemy enemy, Type projectileType, float damage, Vector3 origin, Transform firingPoint);

        public virtual void Launch(List<EntityBaseEnemy> enemies, Type projectileType, float damage, Vector3 origin, Transform[] firingPoints)
        {
            int count = enemies.Count;
            int currentFiringPointIndex = 0;
            int firingPointLength = firingPoints.Length;
            for (int i = 0; i < count; i++)
            {
                EntityBaseEnemy enemy = enemies[i];
                Transform firingPoint = firingPoints[currentFiringPointIndex];
                currentFiringPointIndex = (currentFiringPointIndex + 1) % firingPointLength;
                Launch(enemy, projectileType, damage, origin, firingPoint);
            }
        }

        public virtual void Launch(EntityBaseEnemy enemy, Type projectileType, float damage, Vector3 origin, Transform[] firingPoints)
        {
            Launch(enemy, projectileType, damage, origin, GetRandomTransform(firingPoints));
        }

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

        public Transform GetRandomTransform(Transform[] launchPoints)
        {
            int index = UnityEngine.Random.Range(0, launchPoints.Length);
            return launchPoints[index];
        }
    }
}


