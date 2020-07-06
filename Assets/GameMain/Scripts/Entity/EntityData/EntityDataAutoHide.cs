using System;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    [Serializable]
    public class EntityDataAutoHide : EntityData
    {
        public float Time
        {
            get;
            private set;
        }

        public EntityDataAutoHide() : base()
        {
            Time = 0;
        }

        public static EntityDataAutoHide Create(float time, object userData = null)
        {
            EntityDataAutoHide entityDataAutoHide = ReferencePool.Acquire<EntityDataAutoHide>();
            entityDataAutoHide.Time = time;
            return entityDataAutoHide;
        }

        public static EntityDataAutoHide Create(float time, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataAutoHide entityDataAutoHide = ReferencePool.Acquire<EntityDataAutoHide>();
            entityDataAutoHide.Time = time;
            entityDataAutoHide.Position = position;
            entityDataAutoHide.Rotation = rotation;
            return entityDataAutoHide;
        }

        public override void Clear()
        {
            base.Clear();
            Time = 0;
        }
    }
}


