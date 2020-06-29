using System;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    [Serializable]
    public class EntityDataTowerPreview : EntityData
    {
        public TowerData TowerData
        {
            get;
            private set;
        }

        public EntityDataTowerPreview() : base()
        {
            TowerData = null;
        }

        public static EntityDataTowerPreview Create(TowerData towerData, object userData = null)
        {
            EntityDataTowerPreview entityData = ReferencePool.Acquire<EntityDataTowerPreview>();
            entityData.TowerData = towerData;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            TowerData = null;
        }
    }
}


