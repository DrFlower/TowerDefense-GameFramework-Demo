using System;
using System.Collections.Generic;

namespace GameFramework.Data
{
    internal sealed partial class DataManager : GameFrameworkModule, IDataManager
    {
        private Dictionary<Type, DataInfo> m_dicDataInfos;
        private GameFrameworkLinkedList<DataInfo> m_linkedListDataInfos;

        public int DataCount
        {
            get
            {
                return m_dicDataInfos == null ? 0 : m_dicDataInfos.Count;
            }
        }

        public DataManager()
        {
            m_dicDataInfos = new Dictionary<Type, DataInfo>();
            m_linkedListDataInfos = new GameFrameworkLinkedList<DataInfo>();
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        public void AddData<T>() where T : DataBase
        {
            AddData<T>(Constant.DefaultPriority);
        }

        public void AddData<T>(int priority) where T : DataBase
        {
            Type type = typeof(T);

            if (m_dicDataInfos.ContainsKey(type))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data Type '{0}' is already exist.", type.ToString()));
            }

            DataBase data = (DataBase)Activator.CreateInstance(type);
            if (data == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not create data '{0}'.", type.FullName));
            }

            AddData(data, priority);
        }

        public void AddData(DataBase data)
        {
            AddData(data, Constant.DefaultPriority);
        }

        public void AddData(DataBase data, int priority)
        {
            if (data == null)
            {
                throw new GameFrameworkException("Can not add null data");
            }

            DataInfo dataInfo = DataInfo.Create(data);
            dataInfo.Priority = priority;

            LinkedListNode<DataInfo> current = m_linkedListDataInfos.First;
            while (current != null)
            {
                if (dataInfo.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_linkedListDataInfos.AddBefore(current, dataInfo);
            }
            else
            {
                m_linkedListDataInfos.AddLast(dataInfo);
            }
        }

        public void RemoveData<T>() where T : DataBase
        {
            Type type = typeof(T);

            DataInfo dataInfo = null;

            if (!m_dicDataInfos.TryGetValue(type, out dataInfo))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data Type '{0}' is not exist.", type.ToString()));
            }

            if (dataInfo != null && dataInfo.Data != null)
            {
                RemoveData(dataInfo.Data);
            }
        }

        public void RemoveData(DataBase data)
        {
            if (data == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Data '{0}' is null.", data.Name.ToString()));
            }

            Type type = data.GetType();

            if (!m_dicDataInfos.ContainsKey(type))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data Type '{0}' is not exist.", type.ToString()));
            }

            data.OnUnload();
            data.Shutdown();

            m_dicDataInfos.Remove(type);
        }

        public DataBase[] GetAllData()
        {
            if (m_dicDataInfos != null)
            {
                DataBase[] dataBases = new DataBase[m_linkedListDataInfos.Count];


                LinkedListNode<DataInfo> current = m_linkedListDataInfos.First;
                int index = 0;
                while (current != null)
                {
                    dataBases[index] = current.Value.Data;
                    index++;
                    current = current.Next;
                }

                return dataBases;
            }

            return null;
        }

        public void GetAllData(List<DataBase> result)
        {
            if (result == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            result.Clear();

            if (m_dicDataInfos != null)
            {
                LinkedListNode<DataInfo> current = m_linkedListDataInfos.First;
                while (current != null)
                {
                    result.Add(current.Value.Data);
                    current = current.Next;
                }
            }
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

