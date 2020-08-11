using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using GameFramework;

namespace Flower.Data
{
    public sealed class DataTower : DataBase
    {
        private IDataTable<DRTower> dtTower;
        private IDataTable<DRTowerLevel> dtTowerLevel;

        private Dictionary<int, TowerData> dicTowerData;
        private Dictionary<int, TowerLevelData> dicTowerLevelData;
        private Dictionary<int, Tower> dicTower;

        private int serialId = 0;

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
            DataProjectile dataProjectile = GameEntry.Data.GetData<DataProjectile>();
            if (dataProjectile == null)
            {
                Log.Error("Can't load DataProjectile");
                return;
            }

            dtTower = GameEntry.DataTable.GetDataTable<DRTower>();
            if (dtTower == null)
                throw new System.Exception("Can not get data table Tower");

            dtTowerLevel = GameEntry.DataTable.GetDataTable<DRTowerLevel>();
            if (dtTowerLevel == null)
                throw new System.Exception("Can not get data table TowerLevel");

            dicTowerData = new Dictionary<int, TowerData>();
            dicTowerLevelData = new Dictionary<int, TowerLevelData>();
            dicTower = new Dictionary<int, Tower>();

            DRTowerLevel[] drTowerLevels = dtTowerLevel.GetAllDataRows();
            foreach (var drTowerLevel in drTowerLevels)
            {
                if (dicTowerLevelData.ContainsKey(drTowerLevel.Id))
                {
                    throw new System.Exception(string.Format("Data tower level id '{0}' duplicate.", drTowerLevel.Id));
                }

                ProjectileData projectileData = dataProjectile.GetProjectileData(drTowerLevel.ProjectileData);
                TowerLevelData towerLevelData = new TowerLevelData(drTowerLevel, projectileData);
                dicTowerLevelData.Add(drTowerLevel.Id, towerLevelData);
            }

            DRTower[] drTowers = dtTower.GetAllDataRows();
            foreach (var drTower in drTowers)
            {
                TowerLevelData[] towerLevelDatas = new TowerLevelData[drTower.Levels.Length];
                for (int i = 0; i < drTower.Levels.Length; i++)
                {
                    if (!dicTowerLevelData.ContainsKey(drTower.Levels[i]))
                    {
                        throw new System.Exception(string.Format("Can not find tower level id '{0}' in DataTable TowerLevel.", drTower.Levels[i]));
                    }

                    towerLevelDatas[i] = dicTowerLevelData[drTower.Levels[i]];
                }

                TowerData towerData = new TowerData(drTower, towerLevelDatas);
                dicTowerData.Add(drTower.Id, towerData);
            }
        }

        private int GenrateSerialId()
        {
            return ++serialId;
        }

        public TowerData GetTowerData(int id)
        {
            if (!dicTowerData.ContainsKey(id))
            {
                Log.Error("Can not find tower data id '{0}'.", id);
                return null;
            }

            return dicTowerData[id];
        }

        public Tower CreateTower(int towerId, int level = 0)
        {
            if (!dicTowerData.ContainsKey(towerId))
            {
                Log.Error("Can not find tower data id '{0}'.", towerId);
                return null;
            }

            int serialId = GenrateSerialId();
            Tower tower = Tower.Create(dicTowerData[towerId], serialId, level);
            dicTower.Add(serialId, tower);

            return tower;
        }

        public void DestroyTower(int serialId)
        {
            if (!dicTower.ContainsKey(serialId))
            {
                Log.Error("Can not find tower serialId '{0}'.", serialId);
                return;
            }

            ReferencePool.Release(dicTower[serialId]);
            dicTower.Remove(serialId);
        }

        public void DestroyTower(Tower tower)
        {
            DestroyTower(tower.SerialId);
        }

        public void UpgradeTower(int serialId)
        {
            if (!dicTower.ContainsKey(serialId))
            {
                Log.Error("Can not find tower serialId '{0}'.", serialId);
                return;
            }

            Tower tower = dicTower[serialId];
            if (tower.Level >= tower.MaxLevel)
            {
                Log.Error("Tower (id:'{0}') has reached the highest level", serialId);
                return;
            }

            DataPlayer dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            int needEnergy = tower.GetBuildEnergy(tower.Level + 1);
            if (dataPlayer.Energy < needEnergy)
            {
                Log.Error("Energy lack,need {0},current is {1}", needEnergy, dataPlayer.Energy);
                return;
            }

            dataPlayer.AddEnergy(-needEnergy);

            int lastLevel = tower.Level;

            tower.Upgrade();
            GameEntry.Event.Fire(this, UpgradeTowerEventArgs.Create(tower, lastLevel));
        }

        public void UpgradeTower(Tower tower)
        {
            UpgradeTower(tower.SerialId);
        }

        public void SellTower(int serialId)
        {
            if (!dicTower.ContainsKey(serialId))
            {
                Log.Error("Can not find tower serialId '{0}'.", serialId);
                return;
            }

            DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();

            if (dataLevel.LevelState != EnumLevelState.Prepare && dataLevel.LevelState != EnumLevelState.Normal)
            {
                return;
            }

            Tower tower = dicTower[serialId];
            GameEntry.Event.Fire(this, HideTowerInLevelEventArgs.Create(tower.SerialId));

            DataPlayer dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            if (dataLevel.LevelState == EnumLevelState.Prepare)
            {
                dataPlayer.AddEnergy(tower.TotalCostEnergy);
            }
            else if (dataLevel.LevelState == EnumLevelState.Normal)
            {
                dataPlayer.AddEnergy(tower.SellEnergy);
            }
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRTower>();
            GameEntry.DataTable.DestroyDataTable<DRTowerLevel>();

            dicTowerData = null;
            dicTowerLevelData = null;

            if (dicTower != null)
            {
                foreach (var item in dicTower.Values)
                {
                    ReferencePool.Release(item);
                }
                dicTower.Clear();
            }

            serialId = 0;
        }

        protected override void OnShutdown()
        {
        }
    }

}