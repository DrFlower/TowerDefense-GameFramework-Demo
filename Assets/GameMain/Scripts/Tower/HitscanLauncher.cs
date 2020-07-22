using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Flower
{
    public class HitscanLauncher : Launcher
    {

        public ParticleSystem fireParticleSystem;

        public override void Launch(EntityBaseEnemy enemy, int projectileEntityId, Type projectileType, float damage, Vector3 origin, Transform firingPoint)
        {

            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(projectileEntityId, projectileType, null, EntityDataProjectileHitscan.Create(enemy, damage, origin, firingPoint, firingPoint.position, firingPoint.rotation)));
            PlayParticles(fireParticleSystem, firingPoint.position, enemy.transform.position);
        }
    }

}
