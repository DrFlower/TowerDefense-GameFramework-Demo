using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    public class EntityRadiusVisualizer : EntityLogicEx
    {

        public float radiusVisualizerHeight = 0.02f;

        public Vector3 localEuler;


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

    }
}

