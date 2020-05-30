using System;
using System.Collections.Generic;

namespace GameFramework.Data
{
    internal sealed partial class DataManager : GameFrameworkModule, IDataManager
    {
        internal sealed class DataInfo : IReference
        {
            private Data m_Data;
            private DataStatus m_Status;
            private int m_Priority;
            public DataInfo()
            {
                m_Data = null;
                m_Status = DataStatus.None;
            }

            public Data Data
            {
                get
                {
                    return m_Data;
                }
            }

            public DataStatus Status
            {
                get
                {
                    return m_Status;
                }
                set
                {
                    m_Status = value;
                }
            }

            public int Priority
            {
                get
                {
                    return m_Priority;
                }
                set
                {
                    m_Priority = value;
                }
            }

            public static DataInfo Create(Data data)
            {
                if (data == null)
                {
                    throw new GameFrameworkException("Data is invalid.");
                }

                DataInfo dataInfo = ReferencePool.Acquire<DataInfo>();
                dataInfo.m_Data = data;
                dataInfo.m_Status = DataStatus.None;
                dataInfo.Priority = 0;
                return dataInfo;
            }

            public void Clear()
            {
                m_Data = null;
                m_Status = DataStatus.None;
                Priority = 0;
            }
        }
    }
}


