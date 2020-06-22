using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    public sealed class DataItem : DataBase
    {
        private IDataTable<DRItem> dtItem;
        private IDataTable<DRItemGroup> dtItemGroup;

        private Dictionary<int, ItemData> dicItemData;
        private Dictionary<int, ItemGroupData> dicItemGroupData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Item");
            LoadDataTable("ItemGroup");
        }

        protected override void OnLoad()
        {
            dtItem = GameEntry.DataTable.GetDataTable<DRItem>();
            if (dtItem == null)
                throw new System.Exception("Can not get data table Item");

            dtItemGroup = GameEntry.DataTable.GetDataTable<DRItemGroup>();
            if (dtItemGroup == null)
                throw new System.Exception("Can not get data table ItemGroup");

            dicItemData = new Dictionary<int, ItemData>();
            dicItemGroupData = new Dictionary<int, ItemGroupData>();

            DRItem[] drItems = dtItem.GetAllDataRows();
            foreach (var drItem in drItems)
            {
                ItemGroupData itemGroupData = null;
                if (!dicItemGroupData.TryGetValue(drItem.ItemGroupId, out itemGroupData))
                {
                    DRItemGroup dRItemGroup = dtItemGroup.GetDataRow(drItem.ItemGroupId);
                    if (dRItemGroup == null)
                    {
                        throw new System.Exception("Can not find ItemGroup id :" + drItem.ItemGroupId);
                    }
                    PoolParamData poolParamData = GameEntry.Data.GetData<DataPoolParam>().GetPoolParamData(dRItemGroup.PoolParamId);

                    itemGroupData = new ItemGroupData(dRItemGroup, poolParamData);
                    dicItemGroupData.Add(drItem.ItemGroupId, itemGroupData);
                }

                DRAssetsPath dRAssetsPath = GameEntry.Data.GetData<DataAssetsPath>().GetDRAssetsPathByAssetsId(drItem.AssetId);

                ItemData itemData = new ItemData(drItem, dRAssetsPath, itemGroupData);
                dicItemData.Add(drItem.Id, itemData);
            }
        }

        public ItemData GetItemData(int id)
        {
            if (dicItemData.ContainsKey(id))
            {
                return dicItemData[id];
            }

            return null;
        }

        public ItemGroupData GetItemGroupData(int id)
        {
            if (dicItemGroupData.ContainsKey(id))
            {
                return dicItemGroupData[id];
            }

            return null;
        }

        public ItemData[] GetAllItemData()
        {
            int index = 0;
            ItemData[] results = new ItemData[dicItemData.Count];
            foreach (var itemData in dicItemData.Values)
            {
                results[index++] = itemData;
            }

            return results;
        }

        public ItemGroupData[] GetAllItemGroupData()
        {
            int index = 0;
            ItemGroupData[] results = new ItemGroupData[dicItemGroupData.Count];
            foreach (var itemGroupData in dicItemGroupData.Values)
            {
                results[index++] = itemGroupData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRItem>();
            GameEntry.DataTable.DestroyDataTable<DRItemGroup>();

            dtItem = null;
            dtItemGroup = null;
            dicItemData = null;
            dicItemGroupData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}