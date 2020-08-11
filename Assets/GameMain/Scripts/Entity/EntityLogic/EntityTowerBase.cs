using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using Flower.Data;

namespace Flower
{
    public abstract class EntityTowerBase : EntityTargetable, IPause
    {
        protected EntityDataTower entityDataTower;
        protected Entity entityTowerLevel;
        protected EntityTowerLevel entityLogicTowerLevel;

        protected bool pause = false;

        public override EnumAlignment Alignment
        {
            get
            {
                return EnumAlignment.Tower;
            }
        }

        protected override float MaxHP
        {
            get
            {
                if (entityDataTower == null)
                    return 0;
                else
                    return entityDataTower.Tower.MaxHP;
            }
        }

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

            hp = entityDataTower.Tower.MaxHP;

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

            GameEntry.Sound.PlaySound(EnumSound.TDTowerUpgrade);
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

        protected override void Dead()
        {
            base.Dead();

            GameEntry.Event.Fire(this, HideTowerInLevelEventArgs.Create(entityDataTower.Tower.SerialId));
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

