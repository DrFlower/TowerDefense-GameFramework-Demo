using System;
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
        private const int DefaultPriority = 0;

        private IDataManager m_DataManager = null;

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
          
        }

        T GetData<T>() where T : DataBase
        {
            return m_DataManager.GetData<T>();
        }

        DataBase GetData(string name)
        {
            return m_DataManager.GetData(name);
        }

        bool HasData<T>() where T : DataBase
        {
            return m_DataManager.HasData<T>();
        }

        bool HasData(string name)
        {
            return m_DataManager.HasData(name);
        }

        DataBase[] GetAllData()
        {
            return m_DataManager.GetAllData();
        }

        void GetAllData(List<DataBase> result)
        {
            m_DataManager.GetAllData(result);
        }

        void AddData<T>() where T : DataBase
        {
            m_DataManager.AddData<T>();
        }

        void AddData(DataBase dataBase)
        {
            m_DataManager.AddData(dataBase);
        }

        void RemoveData<T>() where T : DataBase
        {
            m_DataManager.RemoveData<T>();
        }

        void RemoveData(DataBase dataBase)
        {
            m_DataManager.RemoveData(dataBase);
        }

        void InitAllData()
        {
            m_DataManager.InitAllData();
        }

        void PreLoadAllData()

        {
            m_DataManager.PreLoadAllData();
        }

        void LoadAllData()
        {
            m_DataManager.LoadAllData();
        }

        void UnLoadAllData()
        {
            m_DataManager.LoadAllData();
        }
    }
}