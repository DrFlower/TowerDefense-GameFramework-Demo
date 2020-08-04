using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityParticle : EntityLogicEx, IPause
    {
        protected ParticleSystem ps;

        protected bool pause = false;
        private float pauseTime;

        protected EntityDataFollower entityDatafollower;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            ps = GetComponentInChildren<ParticleSystem>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDatafollower = userData as EntityDataFollower;
            if (entityDatafollower == null)
            {
                return;
            }

            GameEntry.Sound.PlaySound(entityDatafollower.ShowSound, Entity);

            transform.localScale = entityDatafollower.Scale;

            ps.Play(true);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (entityDatafollower != null && entityDatafollower.Follow != null)
            {
                transform.position = entityDatafollower.Follow.position + entityDatafollower.Offset;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityDatafollower = null;

            transform.localScale = Vector3.one;

            pauseTime = 0;
            ps.Stop(true);
        }

        public void Pause()
        {
            pause = true;
            ps.Pause(true);
            pauseTime = ps.time;
        }

        public void Resume()
        {
            pause = false;
            ps.Play();
            ps.time = pauseTime;
        }
    }
}

