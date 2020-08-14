using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace Flower.Data
{
    public sealed class DataPlayer : DataBase
    {
        public int HP { get; private set; }

        private float energy;

        public float Energy
        {
            get
            {
                DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();
                if (!dataLevel.IsInLevel)
                {
                    Log.Error("Is invaild to get player energy outsiede level scene");
                    return 0;
                }

                return energy;
            }

            private set
            {
                energy = value;
            }
        }


        public bool IsEnableDebugEnergy { get; private set; }
        public float DebugAddEnergyCount { get; private set; }

        protected override void OnInit()
        {
        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            HP = GameEntry.Config.GetInt(Constant.Config.PlayerHP);
            IsEnableDebugEnergy = true;
            DebugAddEnergyCount = 1000;
        }

        public void Damage(int value)
        {
            if (value == 0)
                return;

            int lastHP = HP;
            HP -= value;

            bool gameover = false;

            if (HP <= 0)
            {
                HP = 0;
                gameover = true;
            }

            GameEntry.Event.Fire(this, PlayerHPChangeEventArgs.Create(lastHP, HP));

            if (gameover)
                GameOver();
        }

        public void AddEnergy(float value)
        {
            if (value == 0)
                return;

            float lastEnergy = Energy;
            Energy += value;

            GameEntry.Event.Fire(this, PlayerEnergyChangeEventArgs.Create(lastEnergy, Energy));
        }

        public void DebugAddEnergy()
        {
            AddEnergy(DebugAddEnergyCount);
        }

        public void Reset()
        {
            int lastHP = HP;
            HP = GameEntry.Config.GetInt(Constant.Config.PlayerHP);
            //HP = 100;
            GameEntry.Event.Fire(this, PlayerHPChangeEventArgs.Create(lastHP, HP));

            float lastEnergy = Energy;
            DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();
            if (!dataLevel.IsInLevel)
            {
                Log.Error("Is invaild to get player energy outsiede level scene");
                Energy = lastEnergy;
            }
            else
            {
                LevelData levelData = dataLevel.GetLevelData(dataLevel.CurrentLevelIndex);
                Energy = levelData.InitEnergy;
            }

            GameEntry.Event.Fire(this, PlayerEnergyChangeEventArgs.Create(lastEnergy, Energy));
        }

        public bool BuyTower(int towerId)
        {
            return false;
        }

        public void SellTower()
        {

        }

        private void GameOver()
        {
            GameEntry.Data.GetData<DataLevel>().GameFail();
        }

        protected override void OnUnload()
        {
        }

        protected override void OnShutdown()
        {

        }
    }
}


