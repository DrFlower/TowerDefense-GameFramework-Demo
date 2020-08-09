using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace Flower.Data
{
    public sealed class DataEnemy : DataBase
    {
        private IDataTable<DREnemy> dtEnemy;
        private Dictionary<int, EnemyData> dicEnemyData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Enemy");
        }

        protected override void OnLoad()
        {
            dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            if (dtEnemy == null)
                throw new System.Exception("Can not get data table Enemy");

            DataProjectile dataProjectile = GameEntry.Data.GetData<DataProjectile>();
            if (dataProjectile == null)
            {
                Log.Error("Can't load DataProjectile");
                return;
            }

            dicEnemyData = new Dictionary<int, EnemyData>();

            DREnemy[] dREnemies = dtEnemy.GetAllDataRows();
            foreach (var drEnemy in dREnemies)
            {
                ProjectileData projectileData = dataProjectile.GetProjectileData(drEnemy.ProjectileData);
                EnemyData enemyData = new EnemyData(drEnemy, projectileData);
                dicEnemyData.Add(drEnemy.Id, enemyData);
            }
        }

        public EnemyData GetEnemyData(int id)
        {
            if (dicEnemyData.ContainsKey(id))
            {
                return dicEnemyData[id];
            }

            return null;
        }

        public EnemyData[] GetAllEnemyData()
        {
            int index = 0;
            EnemyData[] results = new EnemyData[dicEnemyData.Count];
            foreach (var enemyData in dicEnemyData.Values)
            {
                results[index++] = enemyData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DREnemy>();

            dtEnemy = null;
            dicEnemyData = null;
        }

        protected override void OnShutdown()
        {
        }
    }
}
