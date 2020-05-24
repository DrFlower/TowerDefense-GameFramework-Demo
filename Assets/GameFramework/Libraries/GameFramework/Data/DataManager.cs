using System;
using System.Collections.Generic;

namespace GameFramework.Data
{
    internal sealed partial class DataManager : GameFrameworkModule, IDataManager
    {
        private Dictionary<Type, DataBase> m_Datas;
        private Dictionary<Type, DataStatus> m_DataStatus;

        public int DataCount
        {
            get
            {
                return m_Datas == null ? 0 : m_Datas.Count;
            }
        }

        public DataManager()
        {
            m_Datas = new Dictionary<Type, DataBase>();
            m_DataStatus = new Dictionary<Type, DataStatus>();
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        public void AddData<T>() where T : DataBase
        {
            Type type = typeof(T);

        }

        public void AddData(DataBase dataBase)
        {
            throw new NotImplementedException();
        }

        public void RemoveData(DataBase dataBase)
        {
            throw new NotImplementedException();
        }

        public DataBase[] GetAllData()
        {
            throw new NotImplementedException();
        }

        public void GetAllData(List<DataBase> result)
        {
            throw new NotImplementedException();
        }

        public T GetData<T>() where T : DataBase
        {
            throw new NotImplementedException();
        }

        public DataBase GetData(string name)
        {
            throw new NotImplementedException();
        }

        public bool HasData<T>() where T : DataBase
        {
            throw new NotImplementedException();
        }

        public bool HasData(string name)
        {
            throw new NotImplementedException();
        }

        public void InitAllData()
        {
   
        }

        public void LoadAllData()
        {

        }

        public void PreLoadAllData()
        {

        }

        public void UnLoadAllData()
        {
     
        }

        internal override void Shutdown()
        {

        }
    }
}

