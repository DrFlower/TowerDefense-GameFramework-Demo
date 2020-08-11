using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower.Data;

namespace Flower
{
    public class EntityMissileArray : EntityTowerAttacker
    {
        public float time;

        private float timer;

        private DataLevel dataLevel;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            dataLevel = GameEntry.Data.GetData<DataLevel>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (dataLevel.LevelState == EnumLevelState.Normal)
            {
                timer += elapseSeconds;

                if (timer > time)
                {
                    GameEntry.Event.Fire(this, HideTowerInLevelEventArgs.Create(entityDataTower.Tower.SerialId));
                }
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            timer = 0;
            dataLevel = null;
        }
    }
}

