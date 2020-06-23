using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityRadiusVisualizer : EntityLogicEx
    {
        public Transform radiusVisualizer;
        public float radiusVisualizerHeight = 0.02f;

        public Vector3 localEuler;

        private EntityDataRadiusVisualiser entityDataRadiusVisualiser;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            radiusVisualizer.localEulerAngles = localEuler;

            entityDataRadiusVisualiser = userData as EntityDataRadiusVisualiser;
            if (entityDataRadiusVisualiser == null)
            {
                Log.Error("EntityDataRadiusVisualiser data is invalid.");
                return;
            }

            radiusVisualizer.localScale = new Vector3(entityDataRadiusVisualiser.Radius, 0, 0);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            entityDataRadiusVisualiser = null;
        }

    }
}

