using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityAutoHide : EntityLogicEx
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

            timer = 0;

            EntityDataAutoHide entityDataAutoHide = userData as EntityDataAutoHide;
            if (entityDataAutoHide == null)
            {
                Log.Error("Entity EntityAutoHide show param vaild.");
            }

            hideTime = entityDataAutoHide.Time;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            timer += elapseSeconds;

            if (timer > hideTime)
            {
                GameEntry.Entity.HideEntity(this.Entity);
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

