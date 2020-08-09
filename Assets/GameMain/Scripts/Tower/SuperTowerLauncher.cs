using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Flower.Data;

namespace Flower
{
    public class SuperTowerLauncher : Launcher
    {
        public ParticleSystem fireParticleSystem;

        public override void Launch(List<EntityTargetable> targets, AttackerData attackerData, ProjectileData projectileData, Vector3 origin, Transform[] firingPoints)
        {
            EntityTargetable target = targets[UnityEngine.Random.Range(0, targets.Count)];
            Transform firingPoint = GetRandomTransform(firingPoints);
            Launch(target, attackerData, projectileData, origin, firingPoint);
        }

        public override void Launch(EntityTargetable target, AttackerData attackerData, ProjectileData projectileData, Vector3 origin, Transform firingPoint)
        {
            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                attackerData.ProjectileEntityId,
                TypeUtility.GetEntityType(attackerData.ProjectileType),
                null,
                EntityDataProjectile.Create(target, projectileData, origin, firingPoint, firingPoint.position, firingPoint.rotation)));

            PlayParticles(fireParticleSystem, firingPoint.position, target.transform.position);
        }
    }

}
