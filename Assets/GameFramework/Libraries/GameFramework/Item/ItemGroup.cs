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
            private readonly GameFrameworkLinkedList<IItem> m_Entities;
            private LinkedListNode<IItem> m_CachedNode;

            /// <summary>
            /// 初始化物品组的新实例。
            /// </summary>
            /// <param name="name">物品组名称。</param>
            /// <param name="instanceAutoReleaseInterval">物品实例对象池自动释放可释放对象的间隔秒数。</param>
            /// <param name="instanceCapacity">物品实例对象池容量。</param>
            /// <param name="instanceExpireTime">物品实例对象池对象过期秒数。</param>
            /// <param name="instancePriority">物品实例对象池的优先级。</param>
            /// <param name="itemGroupHelper">物品组辅助器。</param>
            /// <param name="objectPoolManager">对象池管理器。</param>
            public ItemGroup(string name, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority, IItemGroupHelper itemGroupHelper, IObjectPoolManager objectPoolManager)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Item group name is invalid.");
                }

                if (itemGroupHelper == null)
                {
                    throw new GameFrameworkException("Item group helper is invalid.");
                }

                m_Name = name;
                m_ItemGroupHelper = itemGroupHelper;
                m_InstancePool = objectPoolManager.CreateSingleSpawnObjectPool<ItemInstanceObject>(Utility.Text.Format("Item Instance Pool ({0})", name), instanceCapacity, instanceExpireTime, instancePriority);
                m_InstancePool.AutoReleaseInterval = instanceAutoReleaseInterval;
                m_Entities = new GameFrameworkLinkedList<IItem>();
                m_CachedNode = null;
            }

            /// <summary>
            /// 获取物品组名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取物品组中物品数量。
            /// </summary>
            public int ItemCount
            {
                get
                {
                    return m_Entities.Count;
                }
            }

            /// <summary>
            /// 获取或设置物品组实例对象池自动释放可释放对象的间隔秒数。
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
            /// 获取或设置物品组实例对象池的容量。
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
            /// 获取或设置物品组实例对象池对象过期秒数。
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
            /// 获取或设置物品组实例对象池的优先级。
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
            /// 获取物品组辅助器。
            /// </summary>
            public IItemGroupHelper Helper
            {
                get
                {
                    return m_ItemGroupHelper;
                }
            }

            /// <summary>
            /// 物品组轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                LinkedListNode<IItem> current = m_Entities.First;
                while (current != null)
                {
                    m_CachedNode = current.Next;
                    current.Value.OnUpdate(elapseSeconds, realElapseSeconds);
                    current = m_CachedNode;
                    m_CachedNode = null;
                }
            }

            /// <summary>
            /// 物品组中是否存在物品。
            /// </summary>
            /// <param name="itemId">物品序列编号。</param>
            /// <returns>物品组中是否存在物品。</returns>
            public bool HasItem(int itemId)
            {
                foreach (IItem item in m_Entities)
                {
                    if (item.Id == itemId)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 物品组中是否存在物品。
            /// </summary>
            /// <param name="itemAssetName">物品资源名称。</param>
            /// <returns>物品组中是否存在物品。</returns>
            public bool HasItem(string itemAssetName)
            {
                if (string.IsNullOrEmpty(itemAssetName))
                {
                    throw new GameFrameworkException("Item asset name is invalid.");
                }

                foreach (IItem item in m_Entities)
                {
                    if (item.ItemAssetName == itemAssetName)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 从物品组中获取物品。
            /// </summary>
            /// <param name="itemId">物品序列编号。</param>
            /// <returns>要获取的物品。</returns>
            public IItem GetItem(int itemId)
            {
                foreach (IItem item in m_Entities)
                {
                    if (item.Id == itemId)
                    {
                        return item;
                    }
                }

                return null;
            }

            /// <summary>
            /// 从物品组中获取物品。
            /// </summary>
            /// <param name="itemAssetName">物品资源名称。</param>
            /// <returns>要获取的物品。</returns>
            public IItem GetItem(string itemAssetName)
            {
                if (string.IsNullOrEmpty(itemAssetName))
                {
                    throw new GameFrameworkException("Item asset name is invalid.");
                }

                foreach (IItem item in m_Entities)
                {
                    if (item.ItemAssetName == itemAssetName)
                    {
                        return item;
                    }
                }

                return null;
            }

            /// <summary>
            /// 从物品组中获取物品。
            /// </summary>
            /// <param name="itemAssetName">物品资源名称。</param>
            /// <returns>要获取的物品。</returns>
            public IItem[] GetEntities(string itemAssetName)
            {
                if (string.IsNullOrEmpty(itemAssetName))
                {
                    throw new GameFrameworkException("Item asset name is invalid.");
                }

                List<IItem> results = new List<IItem>();
                foreach (IItem item in m_Entities)
                {
                    if (item.ItemAssetName == itemAssetName)
                    {
                        results.Add(item);
                    }
                }

                return results.ToArray();
            }

            /// <summary>
            /// 从物品组中获取物品。
            /// </summary>
            /// <param name="itemAssetName">物品资源名称。</param>
            /// <param name="results">要获取的物品。</param>
            public void GetEntities(string itemAssetName, List<IItem> results)
            {
                if (string.IsNullOrEmpty(itemAssetName))
                {
                    throw new GameFrameworkException("Item asset name is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (IItem item in m_Entities)
                {
                    if (item.ItemAssetName == itemAssetName)
                    {
                        results.Add(item);
                    }
                }
            }

            /// <summary>
            /// 从物品组中获取所有物品。
            /// </summary>
            /// <returns>物品组中的所有物品。</returns>
            public IItem[] GetAllEntities()
            {
                List<IItem> results = new List<IItem>();
                foreach (IItem item in m_Entities)
                {
                    results.Add(item);
                }

                return results.ToArray();
            }

            /// <summary>
            /// 从物品组中获取所有物品。
            /// </summary>
            /// <param name="results">物品组中的所有物品。</param>
            public void GetAllEntities(List<IItem> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (IItem item in m_Entities)
                {
                    results.Add(item);
                }
            }

            /// <summary>
            /// 往物品组增加物品。
            /// </summary>
            /// <param name="item">要增加的物品。</param>
            public void AddItem(IItem item)
            {
                m_Entities.AddLast(item);
            }

            /// <summary>
            /// 从物品组移除物品。
            /// </summary>
            /// <param name="item">要移除的物品。</param>
            public void RemoveItem(IItem item)
            {
                if (m_CachedNode != null && m_CachedNode.Value == item)
                {
                    m_CachedNode = m_CachedNode.Next;
                }

                if (!m_Entities.Remove(item))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Item group '{0}' not exists specified item '[{1}]{2}'.", m_Name, item.Id.ToString(), item.ItemAssetName));
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

            public void UnspawnItem(IItem item)
            {
                m_InstancePool.Unspawn(item.Handle);
            }

            public void SetItemInstanceLocked(object itemInstance, bool locked)
            {
                if (itemInstance == null)
                {
                    throw new GameFrameworkException("Item instance is invalid.");
                }

                m_InstancePool.SetLocked(itemInstance, locked);
            }

            public void SetItemInstancePriority(object itemInstance, int priority)
            {
                if (itemInstance == null)
                {
                    throw new GameFrameworkException("Item instance is invalid.");
                }

                m_InstancePool.SetPriority(itemInstance, priority);
            }
        }

    }
}