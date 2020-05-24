using GameFramework;
using System;

namespace UnityGameFramework.Runtime
{
    internal sealed class ShowItemInfo : IReference
    {
        private Type m_ItemLogicType;
        private object m_UserData;

        public ShowItemInfo()
        {
            m_ItemLogicType = null;
            m_UserData = null;
        }

        public Type ItemLogicType
        {
            get
            {
                return m_ItemLogicType;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public static ShowItemInfo Create(Type itemLogicType, object userData)
        {
            ShowItemInfo showItemInfo = ReferencePool.Acquire<ShowItemInfo>();
            showItemInfo.m_ItemLogicType = itemLogicType;
            showItemInfo.m_UserData = userData;
            return showItemInfo;
        }

        public void Clear()
        {
            m_ItemLogicType = null;
            m_UserData = null;
        }
    }
}
