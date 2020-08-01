using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityAnimation : EntityLogicEx, IPause
    {
        protected Animation anim;

        protected bool pause = false;

        protected EntityDataFollower entityDatafollower;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            anim = GetComponentInChildren<Animation>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDatafollower = userData as EntityDataFollower;
            if (entityDatafollower == null)
            {
                Log.Error("EntityParticle '{0}' entity data invaild.", Id);
                return;
            }

            transform.localScale = entityDatafollower.Scale;

            anim.Play();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (entityDatafollower.Follow != null)
            {
                transform.position = entityDatafollower.Follow.position + entityDatafollower.Offset;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityDatafollower = null;

            transform.localScale = Vector3.one;
            anim.Stop();
        }

        public void Pause()
        {
            pause = true;
            anim.Stop();
        }

        public void Resume()
        {
            pause = false;
            anim.Play();
        }
    }
}

