using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    public class TowerData
    {
        private DRTower dRTower;
        private TowerLevelData[] towerLevels;

        public int Id
        {
            get
            {
                return dRTower.Id;
            }
        }

        public string Name
        {
            get
            {
                return GameEntry.Localization.GetString(dRTower.NameId);
            }
        }

        public string Icon
        {
            get
            {
                return dRTower.Icon;
            }
        }

        public int EntityId
        {
            get
            {
                return dRTower.EntityId;
            }
        }

        public TowerData(DRTower dRTower, TowerLevelData[] towerLevels)
        {
            this.dRTower = dRTower;
            this.towerLevels = towerLevels;
        }

        public TowerLevelData GetTowerLevelData(int level)
        {
            if (towerLevels == null || level > GetMaxLevel())
                return null;

            return towerLevels[level];
        }

        public int GetMaxLevel()
        {
            return towerLevels == null ? 0 : towerLevels.Length;
        }
    }
}


