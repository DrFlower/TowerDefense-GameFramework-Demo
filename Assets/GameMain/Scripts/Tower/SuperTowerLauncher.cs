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

        public override void Launch(List<EntityEnemy> enemies, Tower tower, Vector3 origin, Transform[] firingPoints)
        {
            EntityEnemy enemy = enemies[UnityEngine.Random.Range(0, enemies.Count)];
            Transform firingPoint = GetRandomTransform(firingPoints);
            Launch(enemy, tower, origin, firingPoint);
        }

        public override void Launch(EntityEnemy enemy, Tower tower, Vector3 origin, Transform firingPoint)
        {
            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                tower.ProjectileEntityId,
                TypeUtility.GetEntityType(tower.ProjectileType),
                null,
                EntityDataProjectile.Create(enemy, tower, origin, firingPoint, firingPoint.position, firingPoint.rotation)));

            PlayParticles(fireParticleSystem, firingPoint.position, enemy.transform.position);
        }
    }

}
