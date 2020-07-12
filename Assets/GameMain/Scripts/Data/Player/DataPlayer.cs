using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    public sealed class DataPlayer : DataBase
    {
        public int HP { get; private set; }
        public int Energy { get; private set; }
        public bool IsEnableDebugEnergy { get; private set; }
        public int DebugAddEnergyCount { get; private set; }

        protected override void OnInit()
        {
        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            HP = 10;
            Energy = 20;
            IsEnableDebugEnergy = true;
            DebugAddEnergyCount = 1000;
        }

        public void Damage(int value = 1)
        {
            if (value == 0)
                return;

            int lastHP = HP;
            HP -= value;

            bool gameover = false;

            if (HP < 0)
            {
                HP = 0;
                gameover = true;
            }

            GameEntry.Event.Fire(this, PlayerHPChangeEventArgs.Create(lastHP, HP));

            if (gameover)
                Gameover();
        }

        public void AddEnergy(int value)
        {
            if (value == 0)
                return;

            int lastEnergy = Energy;
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
            HP = 100;
            GameEntry.Event.Fire(this, PlayerEnergyChangeEventArgs.Create(lastHP, HP));

            int lastEnergy = Energy;
            Energy = 0;
            GameEntry.Event.Fire(this, PlayerEnergyChangeEventArgs.Create(lastEnergy, Energy));
        }

        public bool BuyTower(int towerId)
        {
            return false;
        }

        public void SellTower()
        {

        }

        private void Gameover()
        {
            GameEntry.Data.GetData<DataLevel>().Gameover();
        }

        protected override void OnUnload()
        {
        }

        protected override void OnShutdown()
        {

        }
    }
}


