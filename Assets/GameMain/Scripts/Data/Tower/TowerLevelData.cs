using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    public class TowerLevelData
    {
        private DRTowerLevel dRTowerLevel;
        private ProjectileData projectileData;

        public int Id
        {
            get
            {
                return dRTowerLevel.Id;
            }
        }

        public string Des
        {
            get
            {
                if (string.IsNullOrEmpty(dRTowerLevel.DesId))
                    return string.Empty;

                return GameEntry.Localization.GetString(dRTowerLevel.DesId);
            }
        }

        public string UpgradeDes
        {
            get
            {
                if (string.IsNullOrEmpty(dRTowerLevel.UpgradeDesId))
                    return string.Empty;

                return GameEntry.Localization.GetString(dRTowerLevel.UpgradeDesId);
            }
        }

        public int EntityId
        {
            get
            {
                return dRTowerLevel.EntityId;
            }
        }

        public ProjectileData ProjectileData
        {
            get
            {
                return projectileData;
            }
        }

        public float Damage
        {
            get
            {
                if (projectileData == null)
                    return 0;
                else
                    return projectileData.Damage;
            }
        }

        public float SplashDamage
        {
            get
            {
                if (projectileData == null)
                    return 0;
                else
                    return projectileData.SplashDamage;
            }
        }

        public float SplashRange
        {
            get
            {
                if (projectileData == null)
                    return 0;
                else
                    return projectileData.SplashRange;
            }
        }

        public float FireRate
        {
            get
            {
                return dRTowerLevel.FireRate;
            }
        }

        public float Range
        {
            get
            {
                return dRTowerLevel.Range;
            }
        }

        public float SpeedDownRate
        {
            get
            {
                return dRTowerLevel.SpeedDownRate;
            }
        }

        public float EnergyRaise
        {
            get
            {
                return dRTowerLevel.EnergyRaise;
            }
        }

        public float EnergyRaiseRate
        {
            get
            {
                return dRTowerLevel.EnergyRaiseRate;
            }
        }

        public float DPS
        {
            get
            {
                return (Damage + SplashDamage) * FireRate;
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

        public TowerLevelData(DRTowerLevel dRTowerLevel, ProjectileData projectileData)
        {
            this.dRTowerLevel = dRTowerLevel;
            this.projectileData = projectileData;
        }

    }

}


