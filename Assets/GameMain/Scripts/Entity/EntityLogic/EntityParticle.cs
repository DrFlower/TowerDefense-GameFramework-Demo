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

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            ps = GetComponentInChildren<ParticleSystem>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            ps.Play(true);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

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

