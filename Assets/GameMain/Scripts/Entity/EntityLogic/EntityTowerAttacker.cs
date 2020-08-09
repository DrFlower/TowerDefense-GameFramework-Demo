using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public abstract class EntityTowerAttacker : EntityTowerBase
    {
        private TowerTargetter towerTargetter;
        private TowerAttacker towerAttacker;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            towerTargetter = transform.Find("Targetter").GetComponent<TowerTargetter>();
            towerAttacker = transform.Find("Attack").GetComponent<TowerAttacker>();

            towerTargetter.OnInit(userData);
            towerAttacker.OnInit(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            towerTargetter.OnUpdate(elapseSeconds, realElapseSeconds);
            towerAttacker.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            AttackerData attackerData = AttackerData.Create(entityDataTower.Tower.Range,
                entityDataTower.Tower.FireRate,
                entityDataTower.Tower.IsMultiAttack,
                entityDataTower.Tower.ProjectileType,
                entityDataTower.Tower.ProjectileEntityId
                );
            towerAttacker.SetData(attackerData, entityDataTower.Tower.ProjectileData);

            towerTargetter.OnShow(userData);
            towerAttacker.OnShow(userData);
            towerAttacker.SetOwnerEntity(Entity);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            towerTargetter.OnHide(isShutdown, userData);
            towerAttacker.OnHide(isShutdown, userData);
            towerAttacker.EmptyOwnerEntity();
        }

        protected override void OnShowTowerLevelSuccess(Entity entity)
        {
            base.OnShowTowerLevelSuccess(entity);

            EntityTowerLevel entityTowerLevel = entity.Logic as EntityTowerLevel;
            towerTargetter.SetTurret(entityTowerLevel.turret);
            towerTargetter.SetSearchRange(entityDataTower.Tower.Range);
            towerTargetter.ResetTargetter();

            AttackerData attackerData = AttackerData.Create(entityDataTower.Tower.Range,
                entityDataTower.Tower.FireRate,
                entityDataTower.Tower.IsMultiAttack,
                entityDataTower.Tower.ProjectileType,
                entityDataTower.Tower.ProjectileEntityId
                );

            towerAttacker.SetData(attackerData, entityDataTower.Tower.ProjectileData);
            towerAttacker.SetTowerTargetter(towerTargetter);
            towerAttacker.SetProjectilePoints(entityTowerLevel.projectilePoints);
            towerAttacker.SetEpicenter(entityTowerLevel.epicenter);
            towerAttacker.SetLaunch(entityTowerLevel.launcher);
            towerAttacker.ResetAttack();
        }
    }
}

