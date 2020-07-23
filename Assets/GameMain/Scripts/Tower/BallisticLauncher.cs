using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Flower
{
    public class BallisticLauncher : Launcher
    {
        public ParticleSystem fireParticleSystem;

        public override void Launch(EntityBaseEnemy enemy, int projectileEntityId, Type projectileType, float damage, Vector3 origin, Transform firingPoint)
        {
            Vector3 startPosition = firingPoint.position;
            Vector3 targetPoint;
            //if (ballisticProjectile.fireMode == BallisticFireMode.UseLaunchSpeed)
            //{
            //    // use speed
            //    targetPoint = Ballistics.CalculateBallisticLeadingTargetPointWithSpeed(
            //        startPosition,
            //        enemy.transform.position, enemy.velocity,
            //        ballisticProjectile.startSpeed, ballisticProjectile.arcPreference, Physics.gravity.y, 4);
            //}
            //else
            //{
            //    // use angle
            //    targetPoint = Ballistics.CalculateBallisticLeadingTargetPointWithAngle(
            //        startPosition,
            //        enemy.position, enemy.velocity, ballisticProjectile.firingAngle,
            //        ballisticProjectile.arcPreference, Physics.gravity.y, 4);
            //}
            //ballisticProjectile.FireAtPoint(startPosition, targetPoint);
            //ballisticProjectile.IgnoreCollision(LevelManager.instance.environmentColliders);

            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(projectileEntityId, projectileType, null, EntityDataProjectileBallistic.Create(enemy, damage, origin, firingPoint, firingPoint.position, firingPoint.rotation)));

            PlayParticles(fireParticleSystem, firingPoint.position, enemy.transform.position);
        }
    }
}


