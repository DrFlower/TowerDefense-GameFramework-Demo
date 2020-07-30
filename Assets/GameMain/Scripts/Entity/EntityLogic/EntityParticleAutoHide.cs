using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityParticleAutoHide : EntityParticle
    {
        private float hideTime = 0;
        private float timer = 0;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            hideTime = ps.main.duration;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            timer += elapseSeconds;

            if (timer > hideTime)
            {
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(Entity.Id));
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            timer = 0;
            hideTime = 0;
        }
    }
}

