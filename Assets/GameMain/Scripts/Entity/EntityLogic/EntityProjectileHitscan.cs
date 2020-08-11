using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityProjectileHitscan : EntityProjectile
    {
        public ParticleSystem projectileParticles;
        public float delayTime = 0f;
        private float timer;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            projectileParticles.transform.position = entityDataProjectile.FiringPoint.position;
            projectileParticles.transform.LookAt(entityDataProjectile.EntityTargetable.transform.position);
            projectileParticles.Play();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            timer += elapseSeconds;

            if (timer >= delayTime)
            {
                AttackTarget();
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

        private void AttackTarget()
        {
            SpawnCollisionParticles();

            if (!entityDataProjectile.EntityTargetable.IsDead)
                entityDataProjectile.EntityTargetable.Damage(entityDataProjectile.ProjectileData.Damage);
        }

    }
}