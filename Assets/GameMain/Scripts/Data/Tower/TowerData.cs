using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

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

        public int PreviewEntityId
        {
            get
            {
                return dRTower.PreviewEntityId;
            }
        }

        public int ProjectileEntityId
        {
            get
            {
                return dRTower.ProjectileEntityId;
            }
        }

        public string ProjectileType
        {
            get
            {
                return dRTower.ProjectileType;
            }
        }

        public bool IsMultiAttack
        {
            get
            {
                return dRTower.IsMultiAttack;
            }
        }

        public float MaxHP
        {
            get
            {
                return dRTower.MaxHP;
            }
        }

        public IntVector2 Dimensions
        {
            get;
            private set;
        }

        public string Type
        {
            get
            {
                return dRTower.Type;
            }
        }

        public TowerData(DRTower dRTower, TowerLevelData[] towerLevels)
        {
            this.dRTower = dRTower;
            this.towerLevels = towerLevels;

            int[] dimensions = dRTower.Dimensions;
            if (dimensions == null || dimensions.Length < 2)
            {
                Log.Error("Tower ('{0}') dimensions data invaild", dRTower.Id);
                return;
            }

            Dimensions = new IntVector2(dimensions[0], dimensions[1]);
        }

        public TowerLevelData GetTowerLevelData(int level)
        {
            if (towerLevels == null || level > GetMaxLevel())
                return null;

            return towerLevels[level];
        }

        public int GetMaxLevel()
        {
            return towerLevels == null ? 0 : towerLevels.Length - 1;
        }
    }
}


