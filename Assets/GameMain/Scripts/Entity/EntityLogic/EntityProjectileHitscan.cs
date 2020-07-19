using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityProjectileHitscan : EntityProjectile
    {
        public ParticleSystem collisionParticles;

        private EntityDataProjectileHitscan entityDataProjectileHitscan;

        private float delayTime = 0.1f;
        private float timer;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDataProjectileHitscan = userData as EntityDataProjectileHitscan;

            if (entityDataProjectileHitscan == null)
            {
                Log.Error("Entity EntityProjectileHitscan '{0}' entity data invaild.", Id);
                return;
            }


        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            timer += elapseSeconds;

            if (timer >= delayTime)
            {
                AttackEnemy();
                GameEntry.Entity.HideEntity(Entity);
            }


        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            timer = 0;
        }

        private void AttackEnemy()
        {
            collisionParticles.transform.position = entityDataProjectileHitscan.EntityEnemy.transform.position;
            collisionParticles.Play();
            entityDataProjectileHitscan.EntityEnemy.Damage(entityDataProjectileHitscan.Damge);
        }

    }
}

