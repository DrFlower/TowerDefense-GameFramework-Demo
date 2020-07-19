using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Flower
{
    public class HitscanLauncher : Launcher
    {

        public ParticleSystem fireParticleSystem;

        public override void Launch(EntityBaseEnemy enemy, Type projectileType, float damage, Vector3 origin, Transform firingPoint)
        {
            

            //var hitscanProjectile = porojectile as EntityProjectileHitscan;
            //if (hitscanProjectile == null)
            //{
            //    return;
            //}
            //hitscanProjectile.transform.position = firingPoint.position;
            //hitscanProjectile.AttackEnemy(firingPoint.position, enemy);
            PlayParticles(fireParticleSystem, firingPoint.position, enemy.transform.position);
        }
    }

}
