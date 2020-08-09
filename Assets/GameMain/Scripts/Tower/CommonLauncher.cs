using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Flower.Data;

namespace Flower
{
    public class CommonLauncher : Launcher
    {
        public ParticleSystem fireParticleSystem;

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
