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
        public Color color;

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

            radiusVisualizer.localScale = Vector3.one * entityDataRadiusVisualiser.Radius * 2.0f;

            var visualizerRenderer = radiusVisualizer.GetComponent<Renderer>();
            if (visualizerRenderer != null)
            {
                visualizerRenderer.material.color = color;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            entityDataRadiusVisualiser = null;
            transform.localPosition = Vector3.zero;
            radiusVisualizer.localScale = Vector3.zero;
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            transform.localPosition = entityDataRadiusVisualiser.Position + new Vector3(0, radiusVisualizerHeight, 0);
        }

    }
}

