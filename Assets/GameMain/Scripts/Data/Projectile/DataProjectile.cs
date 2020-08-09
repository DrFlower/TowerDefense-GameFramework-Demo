using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    public sealed class DataProjectile : DataBase
    {
        private IDataTable<DRProjectile> dtProjectile;
        private Dictionary<int, ProjectileData> dicProjectile;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Projectile");
        }

        protected override void OnLoad()
        {
            dtProjectile = GameEntry.DataTable.GetDataTable<DRProjectile>();
            if (dtProjectile == null)
                throw new System.Exception("Can not get data table PoolParam");

            dicProjectile = new Dictionary<int, ProjectileData>();

            DRProjectile[] drProjectiles = dtProjectile.GetAllDataRows();
            foreach (var drProjectile in drProjectiles)
            {
                ProjectileData projectileData = new ProjectileData(drProjectile);
                dicProjectile.Add(drProjectile.Id, projectileData);
            }
        }

        public ProjectileData GetProjectileData(int id)
        {
            if (dicProjectile.ContainsKey(id))
            {
                return dicProjectile[id];
            }

            return null;
        }

        public ProjectileData[] GetAllProjectileData()
        {
            int index = 0;
            ProjectileData[] results = new ProjectileData[dicProjectile.Count];
            foreach (var poolParamData in dicProjectile.Values)
            {
                results[index++] = poolParamData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRProjectile>();

            dtProjectile = null;
            dicProjectile = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}