using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public class EntityTowerLevel : EntityLogicEx
    {
        public Transform turret;
        public Transform[] projectilePoints;
        public Transform epicenter;
        public Launcher launcher;
        public ParticleSystem effect;

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

