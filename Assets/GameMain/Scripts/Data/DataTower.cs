using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower
{
    public sealed class TowerData
    {
        private DRTower dRTower;
        private DRAssetsPath dRAssetsPath;

        public int Id
        {
            get
            {
                return dRTower.Id;
            }   
        }

        public string NameId
        {
            get
            {
                return GameEntry.Localization.GetString(dRTower.NameId);
            }
        }

        public string AssetPath
        {
            get
            {
                return dRAssetsPath.AssetPath;
            }
        }

        public string Icon
        {
            get
            {
                return dRTower.Icon;
            }
        }

        //public LevelData GetLevelData(int level)
        //{

        //}

        //public LevelData[] GetAllLevelData()
        //{

        //}

        //public int GetMaxLevel()
        //{

        //}

        //public bool IsMaxLevel()
        //{

        //}

    }

    public sealed class TowerLevel
    {
        private DRTowerLevel dRTowerLevel;
        private DRAssetsPath dRAssetsPath;

        public int Id
        {
            get
            {
                return dRTowerLevel.Id;
            }
        }

        public string DesId
        {
            get
            {
                return GameEntry.Localization.GetString(dRTowerLevel.DesId);
            }
        }

        public string AssetPath
        {
            get
            {
                return dRAssetsPath.AssetPath;
            }
        }

        public float DPS
        {
            get
            {
                return dRTowerLevel.DPS;
            }
        }

        public float Range
        {
            get
            {
                return dRTowerLevel.Range;
            }
        }

        public int BuildEnergy
        {
            get
            {
                return dRTowerLevel.BuildEnergy;
            }
        }

        public int SellEnergy
        {
            get
            {
                return dRTowerLevel.SellEnergy;
            }
        }
    }

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