using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower
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
        }

        public void Damage(int value)
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

            GameEntry.Event.Fire(this, PlayerEnergyChangeEventArgs.Create(lastHP, HP));

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


