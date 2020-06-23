using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    public abstract class EntityTowerBase : EntityLogicEx
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }
    }
}

