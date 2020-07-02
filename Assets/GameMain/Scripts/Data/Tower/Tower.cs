using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework;

namespace Flower.Data
{
    public class Tower : IReference
    {
        private TowerData towerData;

        public int Level
        {
            get;
            private set;
        }

        public int SerialId
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return towerData.Name;
            }
        }

        public string Icon
        {
            get
            {
                return towerData.Icon;
            }
        }

        public IntVector2 Dimensions
        {
            get
            {
                return towerData.Dimensions;
            }
        }

        public string Type
        {
            get
            {
                return towerData.Type;
            }
        }

        public int MaxLevel
        {
            get
            {
                return towerData.GetMaxLevel();
            }
        }

        public string Des
        {
            get
            {
                return GetDes(Level);
            }
        }

        public float DPS
        {
            get
            {
                return GetDPS(Level);
            }
        }

        public float Range
        {
            get
            {
                return GetRange(Level);
            }
        }

        public float BuildEnergy
        {
            get
            {
                return GetBuildEnergy(Level);
            }
        }

        public float SellEnergy
        {
            get
            {
                return GetSellEnergy(Level);
            }
        }

        public int EntityId
        {
            get
            {
                return towerData.EntityId;
            }
        }

        public int PreviewEntityId
        {
            get
            {
                return towerData.PreviewEntityId;
            }
        }

        public int LevelEntityId
        {
            get
            {
                return GetLevelEntityId(Level);
            }
        }

        public Tower()
        {
            towerData = null;
            Level = 0;
            SerialId = 0;
        }

        public string GetDes(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return string.Empty;
            }
            else
            {
                return towerData.GetTowerLevelData(level).Des;
            }
        }

        public int GetLevelEntityId(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).EntityId;
            }
        }

        public float GetDPS(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).DPS;
            }
        }

        public float GetRange(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).Range;
            }
        }

        public int GetBuildEnergy(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).BuildEnergy;
            }
        }

        public int GetSellEnergy(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).SellEnergy;
            }
        }

        public void Upgrade()
        {
            if (Level < MaxLevel)
            {
                Level++;
            }
            else
            {
                Log.Error("Tower (serialId:'{0}') has reached the highest level", SerialId);
            }
        }

        public static Tower Create(TowerData towerData, int serialId, int level = 0)
        {
            Tower tower = ReferencePool.Acquire<Tower>();
            tower.towerData = towerData;
            tower.Level = level;
            tower.SerialId = serialId;
            return tower;
        }

        public void Clear()
        {
            towerData = null;
            Level = 0;
            SerialId = 0;
        }
    }
}


