using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public class EntityTowerAttacker : EntityTowerBase
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

            towerTargetter.OnUpdate(elapseSeconds, realElapseSeconds);
            towerAttacker.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            towerTargetter.OnShow(userData);
            towerAttacker.OnShow(userData);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            towerTargetter.OnHide(isShutdown, userData);
            towerAttacker.OnHide(isShutdown, userData);
        }

        protected override void OnShowTowerLevelSuccess(Entity entity)
        {
            base.OnShowTowerLevelSuccess(entity);

            EntityTowerLevel entityTowerLevel = entity.Logic as EntityTowerLevel;
            towerTargetter.SetTurret(entityTowerLevel.turret);
            towerTargetter.SetSearchRange(entityDataTower.Tower.Range);
            towerTargetter.ResetTargetter();

            towerAttacker.SetTowerTargetter(towerTargetter);
            towerAttacker.SetProjectilePoints(entityTowerLevel.projectilePoints);
            towerAttacker.SetEpicenter(entityTowerLevel.epicenter);
            towerAttacker.SetLaunch(entityTowerLevel.launcher);
            towerAttacker.ResetAttack();
        }
    }
}

