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

        }

        public void AddEnergy(int value)
        {

        }

        public void BuyTower()
        {

        }

        public void SellTower()
        {

        }

        protected override void OnUnload()
        {
        }

        protected override void OnShutdown()
        {
        }
    }
}


