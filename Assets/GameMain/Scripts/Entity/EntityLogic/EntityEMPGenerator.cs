using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public class EntityEMPGenerator : EntityTowerBase
    {
        private TowerTargetter towerTargetter;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            towerTargetter = transform.Find("Targetter").GetComponent<TowerTargetter>();

            towerTargetter.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            towerTargetter.OnShow(userData);
            towerTargetter.targetEntersRange += OnTargetEntersRange;
            towerTargetter.targetExitsRange += OnTargetExitsRange;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            towerTargetter.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            towerTargetter.OnHide(isShutdown, userData);
            towerTargetter.targetEntersRange -= OnTargetEntersRange;
            towerTargetter.targetExitsRange -= OnTargetExitsRange;
        }

        protected override void OnShowTowerLevelSuccess(Entity entity)
        {
            base.OnShowTowerLevelSuccess(entity);

            EntityTowerLevel entityTowerLevel = entity.Logic as EntityTowerLevel;
            towerTargetter.SetTurret(entityTowerLevel.turret);
            towerTargetter.SetSearchRange(entityDataTower.Tower.Range);
            towerTargetter.ResetTargetter();
        }

        private void OnTargetEntersRange(EntityBaseEnemy other)
        {

        }

        /// <summary>
        /// Fired when the targetter aquires loses a targetable
        /// </summary>
        private void OnTargetExitsRange(EntityBaseEnemy other)
        {

        }
    }
}

