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
        private List<EntityEnemy> slowList;

        private int? soundSerialId = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            towerTargetter = transform.Find("Targetter").GetComponent<TowerTargetter>();

            slowList = new List<EntityEnemy>();

            towerTargetter.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            towerTargetter.OnShow(userData);

            soundSerialId = GameEntry.Sound.PlaySound(EnumSound.TDEMPIdle, Entity);
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

            if (soundSerialId != null)
            {
                GameEntry.Sound.StopSound((int)soundSerialId);
                soundSerialId = null;
            }

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

        private void OnTargetEntersRange(EntityTargetable target)
        {
            EntityEnemy enemy = target as EntityEnemy;
            if (enemy == null)
                return;

            enemy.ApplySlow(entityDataTower.Tower.SerialId, entityDataTower.Tower.SpeedDownRate);
            slowList.Add(enemy);
            enemy.OnDead += RemoveSlowTarget;
            enemy.OnHidden += RemoveSlowTarget;
        }

        private void OnTargetExitsRange(EntityTargetable enmey)
        {
            RemoveSlowTarget(enmey);
        }

        private void RemoveSlowTarget(EntityTargetable target)
        {
            EntityEnemy enemy = target as EntityEnemy;
            if (enemy == null)
                return;

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

