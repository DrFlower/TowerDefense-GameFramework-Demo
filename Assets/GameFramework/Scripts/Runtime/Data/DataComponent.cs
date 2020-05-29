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

        [SerializeField]
        private string[] m_AvailableDataTypeNames = null;

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
            DataBase[] datas = new DataBase[m_AvailableDataTypeNames.Length];
            for (int i = 0; i < m_AvailableDataTypeNames.Length; i++)
            {
                Type procedureType = Utility.Assembly.GetType(m_AvailableDataTypeNames[i]);
                if (procedureType == null)
                {
                    Log.Error("Can not find data type '{0}'.", m_AvailableDataTypeNames[i]);
                    return;
                }

                datas[i] = (DataBase)Activator.CreateInstance(procedureType);
                if (datas[i] == null)
                {
                    Log.Error("Can not create data instance '{0}'.", m_AvailableDataTypeNames[i]);
                    return;
                }
            }

            for (int i = 0; i < datas.Length; i++)
            {
                m_DataManager.AddData(datas[i]);
            }

            m_DataManager.InitAllData();
        }

        public T GetData<T>() where T : DataBase
        {
            return m_DataManager.GetData<T>();
        }

        public DataBase GetData(string name)
        {
            return m_DataManager.GetData(name);
        }

        public bool HasData<T>() where T : DataBase
        {
            return m_DataManager.HasData<T>();
        }

        public bool HasData(string name)
        {
            return m_DataManager.HasData(name);
        }

        public DataBase[] GetAllData()
        {
            return m_DataManager.GetAllData();
        }

        public void GetAllData(List<DataBase> result)
        {
            m_DataManager.GetAllData(result);
        }

        public void AddData<T>() where T : DataBase
        {
            m_DataManager.AddData<T>();
        }

        public void AddData(DataBase dataBase)
        {
            m_DataManager.AddData(dataBase);
        }

        public void RemoveData<T>() where T : DataBase
        {
            m_DataManager.RemoveData<T>();
        }

        public void RemoveData(DataBase dataBase)
        {
            m_DataManager.RemoveData(dataBase);
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