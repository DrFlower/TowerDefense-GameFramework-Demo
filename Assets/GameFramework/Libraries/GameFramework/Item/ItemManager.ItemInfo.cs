using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;


namespace GameFramework.Item
{
    internal sealed partial class ItemManager : GameFrameworkModule, IItemManager
    {
        private class ItemInfo : IReference
        {
            private IItem m_Item;
            private ItemStatus m_Status;

            public ItemInfo()
            {
                m_Item = null;
                m_Status = ItemStatus.WillInit;
            }

            public IItem Item
            {
                get
                {
                    return m_Item;
                }
            }

            public ItemStatus Status
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

            public static ItemInfo Create(IItem item)
            {
                if (item == null)
                {
                    throw new GameFrameworkException("Item is invalid.");
                }

                ItemInfo itemInfo = ReferencePool.Acquire<ItemInfo>();
                itemInfo.m_Item = item;
                itemInfo.m_Status = ItemStatus.WillInit;
                return itemInfo;
            }

            public void Clear()
            {
                m_Item = null;
                m_Status = ItemStatus.Unknown;
            }



        }
    }

}


