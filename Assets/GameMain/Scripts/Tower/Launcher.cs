using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Flower.Data;

namespace Flower
{
    public abstract class Launcher : MonoBehaviour, ILauncher
    {
        public abstract void Launch(EntityTargetable target, AttackerData attackerData, ProjectileData projectileData, Vector3 origin, Transform firingPoint);

        public virtual void Launch(List<EntityTargetable> targets, AttackerData attackerData, ProjectileData projectileData, Vector3 origin, Transform[] firingPoints)
        {
            int count = targets.Count;
            int currentFiringPointIndex = 0;
            int firingPointLength = firingPoints.Length;
            for (int i = 0; i < count; i++)
            {
                EntityTargetable target = targets[i];
                Transform firingPoint = firingPoints[currentFiringPointIndex];
                currentFiringPointIndex = (currentFiringPointIndex + 1) % firingPointLength;
                Launch(target, attackerData, projectileData, origin, firingPoint);
            }
        }

        public virtual void Launch(EntityTargetable target, AttackerData attackerData, ProjectileData projectileData, Vector3 origin, Transform[] firingPoints)
        {
            Launch(target, attackerData, projectileData, origin, GetRandomTransform(firingPoints));
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


