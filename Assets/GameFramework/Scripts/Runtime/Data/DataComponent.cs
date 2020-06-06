using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Data;
using UnityEngine;
using GameFramework;

namespace UnityGameFramework.Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Data")]
    public sealed partial class DataComponent : GameFrameworkComponent
    {
        private IDataManager m_DataManager = null;

        [HideInInspector]
        public DataItem[] dataItems = null;

        int DataCount
        {
            get
            {
                return m_DataManager.DataCount;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            m_DataManager = GameFrameworkEntry.GetModule<IDataManager>();
            if (m_DataManager == null)
            {
                Log.Fatal("Data manager is invalid.");
                return;
            }
        }

        private void Start()
        {
            Data[] datas = new Data[dataItems.Length];
            for (int i = 0; i < dataItems.Length; i++)
            {
                Type procedureType = Utility.Assembly.GetType(dataItems[i].dataTypeName);
                if (procedureType == null)
                {
                    Log.Error("Can not find data type '{0}'.", dataItems[i].dataTypeName);
                    return;
                }

                datas[i] = (Data)Activator.CreateInstance(procedureType);
                if (datas[i] == null)
                {
                    Log.Error("Can not create data instance '{0}'.", dataItems[i].dataTypeName);
                    return;
                }
            }

            for (int i = 0; i < datas.Length; i++)
            {
                m_DataManager.AddData(datas[i]);
            }

            m_DataManager.InitAllData();
        }

        public T GetData<T>() where T : Data
        {
            return m_DataManager.GetData<T>();
        }

        public Data GetData(string name)
        {
            return m_DataManager.GetData(name);
        }

        public bool HasData<T>() where T : Data
        {
            return m_DataManager.HasData<T>();
        }

        public bool HasData(string name)
        {
            return m_DataManager.HasData(name);
        }

        public Data[] GetAllData()
        {
            return m_DataManager.GetAllData();
        }

        public void GetAllData(List<Data> result)
        {
            m_DataManager.GetAllData(result);
        }

        public void AddData<T>() where T : Data
        {
            m_DataManager.AddData<T>();
        }

        public void AddData(Data Data)
        {
            m_DataManager.AddData(Data);
        }

        public void RemoveData<T>() where T : Data
        {
            m_DataManager.RemoveData<T>();
        }

        public void RemoveData(Data Data)
        {
            m_DataManager.RemoveData(Data);
        }

        public void InitAllData()
        {
            m_DataManager.InitAllData();
        }

        public void PreLoadAllData()

        {
            m_DataManager.PreLoadAllData();
        }

        public void LoadAllData()
        {
            m_DataManager.LoadAllData();
        }

        public void UnLoadAllData()
        {
            m_DataManager.LoadAllData();
        }
    }
}