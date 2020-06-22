using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;


namespace Flower.Data
{
    public sealed class DataEntity : DataBase
    {
        private IDataTable<DREntity> dtEntity;
        private IDataTable<DREntityGroup> dtEntityGroup;

        private Dictionary<int, EntityData> dicEntityData;
        private Dictionary<int, EntityGroupData> dicEntityGroupData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Entity");
            LoadDataTable("EntityGroup");
        }

        protected override void OnLoad()
        {
            dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            if (dtEntity == null)
                throw new System.Exception("Can not get data table Entity");

            dtEntityGroup = GameEntry.DataTable.GetDataTable<DREntityGroup>();
            if (dtEntityGroup == null)
                throw new System.Exception("Can not get data table EntityGroup");

            dicEntityData = new Dictionary<int, EntityData>();
            dicEntityGroupData = new Dictionary<int, EntityGroupData>();

            DREntity[] drEntitys = dtEntity.GetAllDataRows();
            foreach (var drEntity in drEntitys)
            {
                EntityGroupData entityGroupData = null;
                if (!dicEntityGroupData.TryGetValue(drEntity.EntityGroupId, out entityGroupData))
                {
                    DREntityGroup dREntityGroup = dtEntityGroup.GetDataRow(drEntity.EntityGroupId);
                    if (dREntityGroup == null)
                    {
                        throw new System.Exception("Can not find EntityGroup id :" + drEntity.EntityGroupId);
                    }
                    PoolParamData poolParamData = GameEntry.Data.GetData<DataPoolParam>().GetPoolParamData(dREntityGroup.PoolParamId);

                    entityGroupData = new EntityGroupData(dREntityGroup, poolParamData);
                    dicEntityGroupData.Add(drEntity.EntityGroupId, entityGroupData);
                }

                DRAssetsPath dRAssetsPath = GameEntry.Data.GetData<DataAssetsPath>().GetDRAssetsPathByAssetsId(drEntity.AssetId);

                EntityData entityData = new EntityData(drEntity, dRAssetsPath, entityGroupData);
                dicEntityData.Add(drEntity.Id, entityData);
            }
        }

        public EntityData GetEntityData(int id)
        {
            if (dicEntityData.ContainsKey(id))
            {
                return dicEntityData[id];
            }

            return null;
        }

        public EntityGroupData GetEntityGroupData(int id)
        {
            if (dicEntityGroupData.ContainsKey(id))
            {
                return dicEntityGroupData[id];
            }

            return null;
        }

        public EntityData[] GetAllEntityData()
        {
            int index = 0;
            EntityData[] results = new EntityData[dicEntityData.Count];
            foreach (var entityData in dicEntityData.Values)
            {
                results[index++] = entityData;
            }

            return results;
        }

        public EntityGroupData[] GetAllEntityGroupData()
        {
            int index = 0;
            EntityGroupData[] results = new EntityGroupData[dicEntityGroupData.Count];
            foreach (var entityGroupData in dicEntityGroupData.Values)
            {
                results[index++] = entityGroupData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DREntity>();
            GameEntry.DataTable.DestroyDataTable<DREntityGroup>();

            dtEntity = null;
            dtEntityGroup = null;
            dicEntityData = null;
            dicEntityGroupData = null;
        }

        protected override void OnShutdown()
        {
        }
    }
}
