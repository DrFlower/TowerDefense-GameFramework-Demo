using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    public sealed class PoolParamData
    {
        private DRPoolParam dRPoolParam;

        public int Id
        {
            get
            {
                return dRPoolParam.Id;
            }
        }

        public string GroupName
        {
            get
            {
                return dRPoolParam.GroupName;
            }
        }

        public float InstanceAutoReleaseInterval
        {
            get
            {
                return dRPoolParam.InstanceAutoReleaseInterval;
            }
        }

        public int InstanceCapacity
        {
            get
            {
                return dRPoolParam.InstanceCapacity;
            }
        }

        public float InstanceExpireTime
        {
            get
            {
                return dRPoolParam.InstanceExpireTime;
            }
        }

        public int InstancePriority
        {
            get
            {
                return dRPoolParam.InstancePriority;
            }
        }

        public PoolParamData(DRPoolParam dRPoolParam)
        {
            this.dRPoolParam = dRPoolParam;
        }
    }

    public sealed class DataPoolParam : DataBase
    {
        private IDataTable<DRPoolParam> dtPoolParam;
        private Dictionary<int, PoolParamData> dicPoolParam;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("PoolParam");
        }

        protected override void OnLoad()
        {
            dtPoolParam = GameEntry.DataTable.GetDataTable<DRPoolParam>();
            if (dtPoolParam == null)
                throw new System.Exception("Can not get data table PoolParam");

            dicPoolParam = new Dictionary<int, PoolParamData>();

            DRPoolParam[] dRPoolParams = dtPoolParam.GetAllDataRows();
            foreach (var dRPoolParam in dRPoolParams)
            {
                PoolParamData poolParamData = new PoolParamData(dRPoolParam);
                dicPoolParam.Add(dRPoolParam.Id, poolParamData);
            }
        }

        public PoolParamData GetPoolParamData(int id)
        {
            if (dicPoolParam.ContainsKey(id))
            {
                return dicPoolParam[id];
            }

            return null;
        }

        public PoolParamData[] GetAllPoolParamData()
        {
            int index = 0;
            PoolParamData[] results = new PoolParamData[dicPoolParam.Count];
            foreach (var poolParamData in dicPoolParam.Values)
            {
                results[index++] = poolParamData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRPoolParam>();

            dtPoolParam = null;
            dicPoolParam = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}