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

            dataInfo.Data.OnUnload();
            dataInfo.Data.Shutdown();

            m_dicDataInfos.Remove(type);
        }

        public void RemoveData(DataBase data)
        {
            if (data == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Data '{0}' is null.", data.Name.ToString()));
            }

            Type type = data.GetType();
            DataInfo dataInfo = null;

            if (!m_dicDataInfos.TryGetValue(type,out dataInfo))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data Type '{0}' is not exist.", type.ToString()));
            }

            if(!ReferenceEquals(dataInfo.Data, data))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data '{0}' is not the same instance.", type.ToString()));
            }

            dataInfo.Data.OnUnload();
            dataInfo.Data.Shutdown();

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

            if (m_linkedListDataInfos != null)
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
            Type type = typeof(T);

            DataInfo dataInfo = null;

            if (!m_dicDataInfos.TryGetValue(type, out dataInfo))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data Type '{0}' is not exist.", type.ToString()));
            }

            return (T)dataInfo.Data;
        }

        public DataBase GetData(string name)
        {
            if (string.IsNullOrEmpty(null))
            {
                throw new GameFrameworkException(Utility.Text.Format("Param data name vaild."));
            }

            DataBase data = null;
            foreach (var item in m_dicDataInfos)
            {
                if (item.Value.Data.Name == name)
                    data = item.Value.Data;
            }

            return data;
        }

        public bool HasData<T>() where T : DataBase
        {
            if (string.IsNullOrEmpty(null))
            {
                throw new GameFrameworkException(Utility.Text.Format("Param data name vaild."));
            }

            Type type = typeof(T);

            if (m_dicDataInfos != null)
                return m_dicDataInfos.ContainsKey(type);

            return false;
        }

        public bool HasData(string name)
        {
            if (m_dicDataInfos != null)
            {
                foreach (var item in m_dicDataInfos)
                {
                    if (item.Value.Data.Name == name)
                        return true;
                }
            }

            return false;
        }

        public void InitAllData()
        {
            if (m_linkedListDataInfos == null)
                return;

            if (m_linkedListDataInfos != null)
            {
                LinkedListNode<DataInfo> current = m_linkedListDataInfos.First;
                while (current != null)
                {
                    current.Value.Data.Init();
                    current = current.Next;
                }
            }
        }

        public void PreLoadAllData()
        {
            if (m_linkedListDataInfos != null)
            {
                LinkedListNode<DataInfo> current = m_linkedListDataInfos.First;
                while (current != null)
                {
                    current.Value.Data.OnPreload();
                    current = current.Next;
                }
            }
        }

        public void LoadAllData()
        {
            if (m_linkedListDataInfos != null)
            {
                LinkedListNode<DataInfo> current = m_linkedListDataInfos.First;
                while (current != null)
                {
                    current.Value.Data.OnLoad();
                    current = current.Next;
                }
            }
        }

        public void UnLoadAllData()
        {
            if (m_linkedListDataInfos != null)
            {
                LinkedListNode<DataInfo> current = m_linkedListDataInfos.First;
                while (current != null)
                {
                    current.Value.Data.OnUnload();
                    current = current.Next;
                }
            }
        }

        internal override void Shutdown()
        {
            for (LinkedListNode<DataInfo> current = m_linkedListDataInfos.Last; current != null; current = current.Previous)
            {
                current.Value.Data.Shutdown();
            }

            m_dicDataInfos.Clear();
            m_linkedListDataInfos.Clear();           
        }
    }
}

