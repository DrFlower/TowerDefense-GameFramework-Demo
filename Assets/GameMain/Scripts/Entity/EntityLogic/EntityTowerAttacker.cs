using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public abstract class EntityTowerAttacker : EntityTowerBase
    {
        private Targetter targetter;
        private Attacker attacker;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            targetter = transform.Find("Targetter").GetComponent<Targetter>();
            attacker = transform.Find("Attack").GetComponent<Attacker>();

            targetter.OnInit(userData);
            attacker.OnInit(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            targetter.OnUpdate(elapseSeconds, realElapseSeconds);
            attacker.OnUpdate(elapseSeconds, realElapseSeconds);
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
            attacker.SetData(attackerData, entityDataTower.Tower.ProjectileData);

            targetter.OnShow(userData);
            attacker.OnShow(userData);
            attacker.SetOwnerEntity(Entity);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            targetter.OnHide(isShutdown, userData);
            attacker.OnHide(isShutdown, userData);
            attacker.EmptyOwnerEntity();
        }

        protected override void OnShowTowerLevelSuccess(Entity entity)
        {
            base.OnShowTowerLevelSuccess(entity);

            EntityTowerLevel entityTowerLevel = entity.Logic as EntityTowerLevel;
            targetter.SetAlignment(Alignment);
            targetter.SetTurret(entityTowerLevel.turret);
            targetter.SetSearchRange(entityDataTower.Tower.Range);
            targetter.ResetTargetter();

            AttackerData attackerData = AttackerData.Create(entityDataTower.Tower.Range,
                entityDataTower.Tower.FireRate,
                entityDataTower.Tower.IsMultiAttack,
                entityDataTower.Tower.ProjectileType,
                entityDataTower.Tower.ProjectileEntityId
                );

            attacker.SetData(attackerData, entityDataTower.Tower.ProjectileData);
            attacker.SetTargetter(targetter);
            attacker.SetProjectilePoints(entityTowerLevel.projectilePoints);
            attacker.SetEpicenter(entityTowerLevel.epicenter);
            attacker.SetLaunch(entityTowerLevel.launcher);
            attacker.ResetAttack();
        }
    }
}

