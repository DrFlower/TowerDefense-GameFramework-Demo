
namespace GameFramework.Item
{
    internal sealed partial class ItemManager : GameFrameworkModule, IItemManager
    {
        private sealed class ShowItemInfo : IReference
        {
            private int m_SerialId;
            private int m_ItemId;
            private ItemGroup m_ItemGroup;
            private object m_UserData;

            public ShowItemInfo()
            {
                m_SerialId = 0;
                m_ItemId = 0;
                m_ItemGroup = null;
                m_UserData = null;
            }

            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
            }

            public int ItemId
            {
                get
                {
                    return m_ItemId;
                }
            }

            public ItemGroup ItemGroup
            {
                get
                {
                    return m_ItemGroup;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }

            public static ShowItemInfo Create(int serialId, int itemId, ItemGroup itemGroup, object userData)
            {
                ShowItemInfo showItemInfo = ReferencePool.Acquire<ShowItemInfo>();
                showItemInfo.m_SerialId = serialId;
                showItemInfo.m_ItemId = itemId;
                showItemInfo.m_ItemGroup = itemGroup;
                showItemInfo.m_UserData = userData;
                return showItemInfo;
            }

            public void Clear()
            {
                m_SerialId = 0;
                m_ItemId = 0;
                m_ItemGroup = null;
                m_UserData = null;
            }
        }
    }
}
