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
        private Dictionary<int, Entity> dic;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            towerTargetter = transform.Find("Targetter").GetComponent<TowerTargetter>();

            dic = new Dictionary<int, Entity>();

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

            RemoveAllTarget();
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
            if (other.SlowDown(entityDataTower.Tower.SpeedDownRate))
            {
                dic.Add(other.Id, null);
                GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create((int)EnumEntity.SlowFx,
                    typeof(EntityParticle),
                    (entity) =>
                    {
                        dic[other.Id] = entity;
                    },
                        EntityDataParticle.Create(other.transform,
                        other.EntityDataEnemy.EnemyData.ApplyEffectOffset,
                        Vector3.one * other.EntityDataEnemy.EnemyData.ApplyEffectScale,
                        other.transform.position,
                        other.transform.rotation)
                    )
                    );
            }
        }

        /// <summary>
        /// Fired when the targetter aquires loses a targetable
        /// </summary>
        private void OnTargetExitsRange(EntityBaseEnemy other)
        {
            if (dic.ContainsKey(other.Id))
            {
                if (dic[other.Id] != null)
                {
                    GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(other.Id));
                }
                other.ResumeSpeed();
                dic.Remove(other.Id);
            }
        }

        private void RemoveAllTarget()
        {
            foreach (var item in dic)
            {
                if (item.Value != null)
                {
                    GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(item.Value.Id));
                }
                //item.Key.ResumeSpeed();
            }

            dic.Clear();
        }

    }
}

