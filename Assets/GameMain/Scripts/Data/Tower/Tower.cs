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

        public bool IsMaxLevel
        {
            get
            {
                return Level >= towerData.GetMaxLevel();
            }
        }

        public float MaxHP
        {
            get
            {
                return towerData.MaxHP;
            }
        }

        public string Des
        {
            get
            {
                return GetDes(Level);
            }
        }

        public string UpgradeDes
        {
            get
            {
                return GetUpgradeDes(Level);
            }
        }


        public float Damage
        {
            get
            {
                return GetDamage(Level);
            }
        }

        public float SplashDamage
        {
            get
            {
                return GetSplashDamage(Level);
            }
        }

        public float SplashRange
        {
            get
            {
                return GetSplashRange(Level);
            }
        }

        public float FireRate
        {
            get
            {
                return GetFireRate(Level);
            }
        }

        public float SpeedDownRate
        {
            get
            {
                return GetSpeedDownRate(Level);
            }
        }

        public float EnergyRaise
        {
            get
            {
                return GetEnergyRaise(Level);
            }
        }

        public float EnergyRaiseRate
        {
            get
            {
                return GetEnergyRaiseRate(Level);
            }
        }

        public float DPS
        {
            get
            {
                return GetDPS(Level);
            }
        }

        public ProjectileData ProjectileData
        {
            get
            {
                return GetProjectileData(Level);
            }
        }

        public float Range
        {
            get
            {
                return GetRange(Level);
            }
        }

        public int BuildEnergy
        {
            get
            {
                return GetBuildEnergy(Level);
            }
        }

        public int SellEnergy
        {
            get
            {
                return GetSellEnergy(Level);
            }
        }

        public int TotalCostEnergy
        {
            get
            {
                return GetTotalCostEnergy(Level);
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

        public int ProjectileEntityId
        {
            get
            {
                return towerData.ProjectileEntityId;
            }
        }

        public string ProjectileType
        {
            get
            {
                return towerData.ProjectileType;
            }
        }

        public bool IsMultiAttack
        {
            get
            {
                return towerData.IsMultiAttack;
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

        public string GetUpgradeDes(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return string.Empty;
            }
            else
            {
                return towerData.GetTowerLevelData(level).UpgradeDes;
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

        public ProjectileData GetProjectileData(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return null;
            }
            else
            {
                return towerData.GetTowerLevelData(level).ProjectileData;
            }
        }

        public float GetDamage(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).Damage;
            }
        }

        public float GetSplashDamage(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).SplashDamage;
            }
        }

        public float GetSplashRange(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).SplashRange;
            }
        }

        public float GetFireRate(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).FireRate;
            }
        }

        public float GetSpeedDownRate(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).SpeedDownRate;
            }
        }

        public float GetEnergyRaise(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).EnergyRaise;
            }
        }

        public float GetEnergyRaiseRate(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                return towerData.GetTowerLevelData(level).EnergyRaiseRate;
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

        public int GetTotalCostEnergy(int level)
        {
            if (level < 0 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }
            else
            {
                int energy = 0;
                for (int i = 0; i <= level; i++)
                {
                    energy += GetBuildEnergy(i);
                }

                return energy;
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


