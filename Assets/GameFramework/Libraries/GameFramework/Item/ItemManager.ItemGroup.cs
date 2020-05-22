using GameFramework.ObjectPool;
using System.Collections.Generic;

namespace GameFramework.Item
{
    internal sealed partial class ItemManager : GameFrameworkModule, IItemManager
    {
        private sealed class ItemGroup : IItemGroup
        {
            private readonly string m_Name;
            private readonly IItemGroupHelper m_ItemGroupHelper;
            private readonly IObjectPool<ItemInstanceObject> m_InstancePool;
            private readonly GameFrameworkLinkedList<IItem> m_Items;
            private LinkedListNode<IItem> m_CachedNode;

            /// <summary>
            /// 初始化实体组的新实例。
            /// </summary>
            /// <param name="name">实体组名称。</param>
            /// <param name="instanceAutoReleaseInterval">实体实例对象池自动释放可释放对象的间隔秒数。</param>
            /// <param name="instanceCapacity">实体实例对象池容量。</param>
            /// <param name="instanceExpireTime">实体实例对象池对象过期秒数。</param>
            /// <param name="instancePriority">实体实例对象池的优先级。</param>
            /// <param name="ItemGroupHelper">实体组辅助器。</param>
            /// <param name="objectPoolManager">对象池管理器。</param>
            public ItemGroup(string name, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority, IItemGroupHelper ItemGroupHelper, IObjectPoolManager objectPoolManager)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Item group name is invalid.");
                }

                if (ItemGroupHelper == null)
                {
                    throw new GameFrameworkException("Item group helper is invalid.");
                }

                m_Name = name;
                m_ItemGroupHelper = ItemGroupHelper;
                m_InstancePool = objectPoolManager.CreateSingleSpawnObjectPool<ItemInstanceObject>(Utility.Text.Format("Item Instance Pool ({0})", name), instanceCapacity, instanceExpireTime, instancePriority);
                m_InstancePool.AutoReleaseInterval = instanceAutoReleaseInterval;
                m_Items = new GameFrameworkLinkedList<IItem>();
                m_CachedNode = null;
            }

            /// <summary>
            /// 获取实体组名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取实体组中实体数量。
            /// </summary>
            public int ItemCount
            {
                get
                {
                    return m_Items.Count;
                }
            }

            /// <summary>
            /// 获取或设置实体组实例对象池自动释放可释放对象的间隔秒数。
            /// </summary>
            public float InstanceAutoReleaseInterval
            {
                get
                {
                    return m_InstancePool.AutoReleaseInterval;
                }
                set
                {
                    m_InstancePool.AutoReleaseInterval = value;
                }
            }

            /// <summary>
            /// 获取或设置实体组实例对象池的容量。
            /// </summary>
            public int InstanceCapacity
            {
                get
                {
                    return m_InstancePool.Capacity;
                }
                set
                {
                    m_InstancePool.Capacity = value;
                }
            }

            /// <summary>
            /// 获取或设置实体组实例对象池对象过期秒数。
            /// </summary>
            public float InstanceExpireTime
            {
                get
                {
                    return m_InstancePool.ExpireTime;
                }
                set
                {
                    m_InstancePool.ExpireTime = value;
                }
            }

            /// <summary>
            /// 获取或设置实体组实例对象池的优先级。
            /// </summary>
            public int InstancePriority
            {
                get
                {
                    return m_InstancePool.Priority;
                }
                set
                {
                    m_InstancePool.Priority = value;
                }
            }

            /// <summary>
            /// 获取实体组辅助器。
            /// </summary>
            public IItemGroupHelper Helper
            {
                get
                {
                    return m_ItemGroupHelper;
                }
            }

            /// <summary>
            /// 实体组轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                LinkedListNode<IItem> current = m_Items.First;
                while (current != null)
                {
                    m_CachedNode = current.Next;
                    current.Value.OnUpdate(elapseSeconds, realElapseSeconds);
                    current = m_CachedNode;
                    m_CachedNode = null;
                }
            }

            /// <summary>
            /// 实体组中是否存在实体。
            /// </summary>
            /// <param name="ItemId">实体序列编号。</param>
            /// <returns>实体组中是否存在实体。</returns>
            public bool HasItem(int ItemId)
            {
                foreach (IItem Item in m_Items)
                {
                    if (Item.Id == ItemId)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 实体组中是否存在实体。
            /// </summary>
            /// <param name="ItemAssetName">实体资源名称。</param>
            /// <returns>实体组中是否存在实体。</returns>
            public bool HasItem(string ItemAssetName)
            {
                if (string.IsNullOrEmpty(ItemAssetName))
                {
                    throw new GameFrameworkException("Item asset name is invalid.");
                }

                foreach (IItem Item in m_Items)
                {
                    if (Item.ItemAssetName == ItemAssetName)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 从实体组中获取实体。
            /// </summary>
            /// <param name="ItemId">实体序列编号。</param>
            /// <returns>要获取的实体。</returns>
            public IItem GetItem(int ItemId)
            {
                foreach (IItem Item in m_Items)
                {
                    if (Item.Id == ItemId)
                    {
                        return Item;
                    }
                }

                return null;
            }

            /// <summary>
            /// 从实体组中获取实体。
            /// </summary>
            /// <param name="ItemAssetName">实体资源名称。</param>
            /// <returns>要获取的实体。</returns>
            public IItem GetItem(string ItemAssetName)
            {
                if (string.IsNullOrEmpty(ItemAssetName))
                {
                    throw new GameFrameworkException("Item asset name is invalid.");
                }

                foreach (IItem Item in m_Items)
                {
                    if (Item.ItemAssetName == ItemAssetName)
                    {
                        return Item;
                    }
                }

                return null;
            }

            /// <summary>
            /// 从实体组中获取实体。
            /// </summary>
            /// <param name="ItemAssetName">实体资源名称。</param>
            /// <returns>要获取的实体。</returns>
            public IItem[] GetItems(string ItemAssetName)
            {
                if (string.IsNullOrEmpty(ItemAssetName))
                {
                    throw new GameFrameworkException("Item asset name is invalid.");
                }

                List<IItem> results = new List<IItem>();
                foreach (IItem Item in m_Items)
                {
                    if (Item.ItemAssetName == ItemAssetName)
                    {
                        results.Add(Item);
                    }
                }

                return results.ToArray();
            }

            /// <summary>
            /// 从实体组中获取实体。
            /// </summary>
            /// <param name="ItemAssetName">实体资源名称。</param>
            /// <param name="results">要获取的实体。</param>
            public void GetItems(string ItemAssetName, List<IItem> results)
            {
                if (string.IsNullOrEmpty(ItemAssetName))
                {
                    throw new GameFrameworkException("Item asset name is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (IItem Item in m_Items)
                {
                    if (Item.ItemAssetName == ItemAssetName)
                    {
                        results.Add(Item);
                    }
                }
            }

            /// <summary>
            /// 从实体组中获取所有实体。
            /// </summary>
            /// <returns>实体组中的所有实体。</returns>
            public IItem[] GetAllItems()
            {
                List<IItem> results = new List<IItem>();
                foreach (IItem Item in m_Items)
                {
                    results.Add(Item);
                }

                return results.ToArray();
            }

            /// <summary>
            /// 从实体组中获取所有实体。
            /// </summary>
            /// <param name="results">实体组中的所有实体。</param>
            public void GetAllItems(List<IItem> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (IItem Item in m_Items)
                {
                    results.Add(Item);
                }
            }

            /// <summary>
            /// 往实体组增加实体。
            /// </summary>
            /// <param name="Item">要增加的实体。</param>
            public void AddItem(IItem Item)
            {
                m_Items.AddLast(Item);
            }

            /// <summary>
            /// 从实体组移除实体。
            /// </summary>
            /// <param name="Item">要移除的实体。</param>
            public void RemoveItem(IItem Item)
            {
                if (m_CachedNode != null && m_CachedNode.Value == Item)
                {
                    m_CachedNode = m_CachedNode.Next;
                }

                if (!m_Items.Remove(Item))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Item group '{0}' not exists specified Item '[{1}]{2}'.", m_Name, Item.Id.ToString(), Item.ItemAssetName));
                }
            }

            public void RegisterItemInstanceObject(ItemInstanceObject obj, bool spawned)
            {
                m_InstancePool.Register(obj, spawned);
            }

            public ItemInstanceObject SpawnItemInstanceObject(string name)
            {
                return m_InstancePool.Spawn(name);
            }

            public void UnspawnItem(IItem Item)
            {
                m_InstancePool.Unspawn(Item.Handle);
            }

            public void SetItemInstanceLocked(object ItemInstance, bool locked)
            {
                if (ItemInstance == null)
                {
                    throw new GameFrameworkException("Item instance is invalid.");
                }

                m_InstancePool.SetLocked(ItemInstance, locked);
            }

            public void SetItemInstancePriority(object ItemInstance, int priority)
            {
                if (ItemInstance == null)
                {
                    throw new GameFrameworkException("Item instance is invalid.");
                }

                m_InstancePool.SetPriority(ItemInstance, priority);
            }
        }

    }
}