using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityProjectileHitscan : EntityProjectile
    {
        public ParticleSystem projectileParticles;
        public ParticleSystem collisionParticles;

        private EntityDataProjectileHitscan entityDataProjectileHitscan;

        private float delayTime = 0.15f;
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


            projectileParticles.transform.position = entityDataProjectileHitscan.FiringPoint.position;
            projectileParticles.transform.LookAt(entityDataProjectileHitscan.EntityEnemy.transform.position);
            projectileParticles.Play();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            timer += elapseSeconds;

            if (timer >= delayTime)
            {
                AttackEnemy();
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Entity.Id));
            }


        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            timer = 0;
        }

        private void SetProjectileParticles()
        {

        }

        private void AttackEnemy()
        {
            collisionParticles.transform.position = entityDataProjectileHitscan.EntityEnemy.transform.position;
            collisionParticles.Play();

            if (!entityDataProjectileHitscan.EntityEnemy.IsDead)
                entityDataProjectileHitscan.EntityEnemy.Damage(entityDataProjectileHitscan.Damge);
        }

    }
}