using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower
{
    public sealed class DataTower : DataBase
    {
        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Tower");
            LoadDataTable("TowerLevel");
        }

        protected override void OnLoad()
        {
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRTower>();
            GameEntry.DataTable.DestroyDataTable<DRTowerLevel>();
        }

        protected override void OnShutdown()
        {
        }
    }

}