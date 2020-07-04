using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public abstract class EntityTowerBase : EntityLogicEx
    {
        protected EntityDataTower entityDataTower;
        protected Entity entityTowerLevel;
        protected EntityTowerLevel entityLogicTowerLevel;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDataTower = userData as EntityDataTower;
            if (entityDataTower == null)
            {
                Log.Error("Entity tower '{0}' tower data vaild.", Id);
                return;
            }

            ShowTowerLevelEntity(entityDataTower.Tower.Level);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityDataTower = null;
            entityTowerLevel = null;
            entityLogicTowerLevel = null;
        }

        private void ShowTowerLevelEntity(int level)
        {
            if (entityTowerLevel != null)
                HideEntity(entityTowerLevel);

            int entityTypeId = entityDataTower.Tower.GetLevelEntityId(level);
            ShowEntity<EntityTowerLevel>(entityTypeId,
                (entity) =>
                {
                    entityTowerLevel = entity;
                    entityLogicTowerLevel = entityTowerLevel.Logic as EntityTowerLevel;
                    GameEntry.Entity.AttachEntity(entityTowerLevel, this.Entity);
                },
                EntityData.Create(transform.position, transform.rotation));
        }

    }
}

