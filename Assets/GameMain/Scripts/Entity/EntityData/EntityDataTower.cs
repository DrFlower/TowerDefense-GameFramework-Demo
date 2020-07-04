using System;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    [Serializable]
    public class EntityDataTower : EntityData
    {
        public Tower Tower
        {
            get;
            private set;
        }

        public EntityDataTower() : base()
        {
            Tower = null;
        }

        public static EntityDataTower Create(Tower tower, object userData = null)
        {
            EntityDataTower entityData = ReferencePool.Acquire<EntityDataTower>();
            entityData.Tower = tower;
            return entityData;
        }

        public static EntityDataTower Create(Tower tower, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataTower entityData = ReferencePool.Acquire<EntityDataTower>();
            entityData.Tower = tower;
            entityData.Position = position;
            entityData.Rotation = rotation;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            Tower = null;
        }
    }
}


