﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using Flower.Data;

namespace Flower
{
    public abstract class EntityTowerBase : EntityLogicEx, IPause
    {
        protected EntityDataTower entityDataTower;
        protected Entity entityTowerLevel;
        protected EntityTowerLevel entityLogicTowerLevel;

        protected bool pause = false;

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
                Log.Error("Entity tower '{0}' tower data invaild.", Id);
                return;
            }

            ShowTowerLevelEntity(entityDataTower.Tower.Level);

            Subscribe(UpgradeTowerEventArgs.EventId, OnUpgradeTower);
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
                OnShowTowerLevelSuccess,
                EntityData.Create(transform.position, transform.rotation));
        }

        protected virtual void OnShowTowerLevelSuccess(Entity entity)
        {
            entityTowerLevel = entity;
            entityLogicTowerLevel = entityTowerLevel.Logic as EntityTowerLevel;
            GameEntry.Entity.AttachEntity(entityTowerLevel, this.Entity);

            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                (int)EnumEntity.BuildPfx,
                typeof(EntityParticleAutoHide),
                null,
                EntityData.Create(transform.position, transform.rotation)));
        }

        public void ShowControlForm()
        {
            GameEntry.UI.OpenUIForm(EnumUIForm.UITowerControllerForm, entityDataTower.Tower);
        }

        private void OnUpgradeTower(object sender, GameEventArgs e)
        {
            UpgradeTowerEventArgs ne = (UpgradeTowerEventArgs)e;
            if (ne == null)
                return;

            if (ne.Tower.SerialId != entityDataTower.Tower.SerialId)
                return;

            ShowTowerLevelEntity(ne.Tower.Level);
        }

        public void Pause()
        {
            pause = true;
        }

        public void Resume()
        {
            pause = false;
        }
    }
}

