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
        private List<EntityBaseEnemy> slowList;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            towerTargetter = transform.Find("Targetter").GetComponent<TowerTargetter>();

            slowList = new List<EntityBaseEnemy>();

            towerTargetter.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            towerTargetter.OnShow(userData);
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
            RemoveAllTarget();

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

            towerTargetter.targetEntersRange += OnTargetEntersRange;
            towerTargetter.targetExitsRange += OnTargetExitsRange;

            foreach (var item in slowList)
            {
                item.ApplySlow(entityDataTower.Tower.SerialId, entityDataTower.Tower.SpeedDownRate);
            }
        }

        private void OnTargetEntersRange(EntityBaseEnemy enemy)
        {
            enemy.ApplySlow(entityDataTower.Tower.SerialId, entityDataTower.Tower.SpeedDownRate);
            slowList.Add(enemy);
            enemy.OnDead += RemoveSlowTarget;
            enemy.OnHidden += RemoveSlowTarget;
        }

        private void OnTargetExitsRange(EntityBaseEnemy enmey)
        {
            RemoveSlowTarget(enmey);
        }

        private void RemoveSlowTarget(EntityBaseEnemy enemy)
        {
            enemy.RemoveSlow(entityDataTower.Tower.SerialId);

            enemy.OnDead -= RemoveSlowTarget;
            enemy.OnHidden -= RemoveSlowTarget;

            slowList.Remove(enemy);
        }

        private void RemoveAllTarget()
        {
            foreach (var item in slowList)
            {
                item.RemoveSlow(entityDataTower.Tower.SerialId);

                item.OnDead -= RemoveSlowTarget;
                item.OnHidden -= RemoveSlowTarget;
            }

            slowList.Clear();
        }
    }
}

