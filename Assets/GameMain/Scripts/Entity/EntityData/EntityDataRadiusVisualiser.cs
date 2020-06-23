using System;
using UnityEngine;
using GameFramework;

namespace Flower
{
    [Serializable]
    public class EntityDataRadiusVisualiser : EntityData
    {
        public float Radius
        {
            get;
            private set;
        }

        public EntityDataRadiusVisualiser() : base()
        {
            Radius = 0f;
        }

        public static EntityDataRadiusVisualiser Create(float radius, object userData = null)
        {
            EntityDataRadiusVisualiser entityData = ReferencePool.Acquire<EntityDataRadiusVisualiser>();
            entityData.Radius = radius;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            Radius = 0f;
        }
    }
}


