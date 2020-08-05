using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower.Data;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityEnergyPylon : EntityTowerBase
    {
        private float timer;
        private DataPlayer dataPlayer;
        private DataLevel dataLevel;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataLevel = GameEntry.Data.GetData<DataLevel>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (dataLevel.LevelState == EnumLevelState.Normal)
            {
                timer += elapseSeconds;

                if (timer > (1 / entityDataTower.Tower.EnergyRaiseRate))
                {
                    timer -= (1 / entityDataTower.Tower.EnergyRaiseRate);
                    RaiseEnergy();
                }
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            dataPlayer = null;
            dataLevel = null;
            timer = 0;
        }

        protected override void OnShowTowerLevelSuccess(Entity entity)
        {
            base.OnShowTowerLevelSuccess(entity);
            if (entityLogicTowerLevel != null && entityLogicTowerLevel.effect != null)
            {
                entityLogicTowerLevel.effect.Stop(entityLogicTowerLevel);
                entityLogicTowerLevel.effect.gameObject.SetActive(false);
            }
        }

        private void RaiseEnergy()
        {
            dataPlayer.AddEnergy(entityDataTower.Tower.EnergyRaise);
            GameEntry.Sound.PlaySound(EnumSound.TDCurrency, Entity);
            if (entityLogicTowerLevel != null && entityLogicTowerLevel.effect != null)
            {
                entityLogicTowerLevel.effect.gameObject.SetActive(true);
                entityLogicTowerLevel.effect.Play(true);
            }
        }

    }
}

