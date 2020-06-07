using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Item
{
    internal sealed partial class ItemManager : GameFrameworkModule, IItemManager
    {
        private readonly Dictionary<int, ItemInfo> m_ItemInfos;
        private readonly Dictionary<string, ItemGroup> m_ItemGroups;
        private readonly Dictionary<int, int> m_ItemsBeingLoaded;
        private readonly HashSet<int> m_ItemsToReleaseOnLoad;
        private readonly Queue<ItemInfo> m_RecycleQueue;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IObjectPoolManager m_ObjectPoolManager;
        private IResourceManager m_ResourceManager;
        private IItemHelper m_ItemHelper;
        private int m_Serial;
        private bool m_IsShutdown;
        private EventHandler<ShowItemSuccessEventArgs> m_ShowItemSuccessEventHandler;
        private EventHandler<ShowItemFailureEventArgs> m_ShowItemFailureEventHandler;
        private EventHandler<ShowItemUpdateEventArgs> m_ShowItemUpdateEventHandler;
        private EventHandler<ShowItemDependencyAssetEventArgs> m_ShowItemDependencyAssetEventHandler;
        private EventHandler<HideItemCompleteEventArgs> m_HideItemCompleteEventHandler;

        public ItemManager()
        {
            m_ItemInfos = new Dictionary<int, ItemInfo>();
            m_ItemGroups = new Dictionary<string, ItemGroup>();
            m_ItemsBeingLoaded = new Dictionary<int, int>();
            m_ItemsToReleaseOnLoad = new HashSet<int>();
            m_RecycleQueue = new Queue<ItemInfo>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback, LoadAssetUpdateCallback, LoadAssetDependencyAssetCallback);
            m_ObjectPoolManager = null;
            m_ResourceManager = null;
            m_ItemHelper = null;
            m_Serial = 0;
            m_IsShutdown = false;
            m_ShowItemSuccessEventHandler = null;
            m_ShowItemFailureEventHandler = null;
            m_ShowItemUpdateEventHandler = null;
            m_ShowItemDependencyAssetEventHandler = null;
            m_HideItemCompleteEventHandler = null;
        }

        /// <summary>
        /// 获取物品数量。
        /// </summary>
        public int ItemCount
        {
            get
            {
                return m_ItemInfos.Count;
            }
        }

        /// <summary>
        /// 获取物品组数量。
        /// </summary>
        public int ItemGroupCount
        {
            get
            {
                return m_ItemGroups.Count;
            }
        }

        /// <summary>
        /// 显示物品成功事件。
        /// </summary>
        public event EventHandler<ShowItemSuccessEventArgs> ShowItemSuccess
        {
            add
            {
                m_ShowItemSuccessEventHandler += value;
            }
            remove
            {
                m_ShowItemSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 显示物品失败事件。
        /// </summary>
        public event EventHandler<ShowItemFailureEventArgs> ShowItemFailure
        {
            add
            {
                m_ShowItemFailureEventHandler += value;
            }
            remove
            {
                m_ShowItemFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 显示物品更新事件。
        /// </summary>
        public event EventHandler<ShowItemUpdateEventArgs> ShowItemUpdate
        {
            add
            {
                m_ShowItemUpdateEventHandler += value;
            }
            remove
            {
                m_ShowItemUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 显示物品时加载依赖资源事件。
        /// </summary>
        public event EventHandler<ShowItemDependencyAssetEventArgs> ShowItemDependencyAsset
        {
            add
            {
                m_ShowItemDependencyAssetEventHandler += value;
            }
            remove
            {
                m_ShowItemDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 隐藏物品完成事件。
        /// </summary>
        public event EventHandler<HideItemCompleteEventArgs> HideItemComplete
        {
            add
            {
                m_HideItemCompleteEventHandler += value;
            }
            remove
            {
                m_HideItemCompleteEventHandler -= value;
            }
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (m_RecycleQueue.Count > 0)
            {
                ItemInfo itemInfo = m_RecycleQueue.Dequeue();
                IItem item = itemInfo.Item;
                ItemGroup itemGroup = (ItemGroup)item.ItemGroup;
                if (itemGroup == null)
                {
                    throw new GameFrameworkException("Item group is invalid.");
                }

                itemInfo.Status = ItemStatus.WillRecycle;
                item.OnRecycle();
                itemInfo.Status = ItemStatus.Recycled;
                itemGroup.UnspawnItem(item);
                ReferencePool.Release(itemInfo);
            }

            foreach (KeyValuePair<string, ItemGroup> itemGroup in m_ItemGroups)
            {
                itemGroup.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        internal override void Shutdown()
        {
            m_IsShutdown = true;
            HideAllLoadedItems();
            m_ItemGroups.Clear();
            m_ItemsBeingLoaded.Clear();
            m_ItemsToReleaseOnLoad.Clear();
            m_RecycleQueue.Clear();
        }

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw new GameFrameworkException("Object pool manager is invalid.");
            }

            m_ObjectPoolManager = objectPoolManager;
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置物品辅助器。
        /// </summary>
        /// <param name="itemHelper">物品辅助器。</param>
        public void SetItemHelper(IItemHelper itemHelper)
        {
            if (itemHelper == null)
            {
                throw new GameFrameworkException("Item helper is invalid.");
            }

            m_ItemHelper = itemHelper;
        }

        /// <summary>
        /// 是否存在物品组。
        /// </summary>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <returns>是否存在物品组。</returns>
        public bool HasItemGroup(string itemGroupName)
        {
            if (string.IsNullOrEmpty(itemGroupName))
            {
                throw new GameFrameworkException("Item group name is invalid.");
            }

            return m_ItemGroups.ContainsKey(itemGroupName);
        }

        /// <summary>
        /// 获取物品组。
        /// </summary>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <returns>要获取的物品组。</returns>
        public IItemGroup GetItemGroup(string itemGroupName)
        {
            if (string.IsNullOrEmpty(itemGroupName))
            {
                throw new GameFrameworkException("Item group name is invalid.");
            }

            ItemGroup itemGroup = null;
            if (m_ItemGroups.TryGetValue(itemGroupName, out itemGroup))
            {
                return itemGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有物品组。
        /// </summary>
        /// <returns>所有物品组。</returns>
        public IItemGroup[] GetAllItemGroups()
        {
            int index = 0;
            IItemGroup[] results = new IItemGroup[m_ItemGroups.Count];
            foreach (KeyValuePair<string, ItemGroup> itemGroup in m_ItemGroups)
            {
                results[index++] = itemGroup.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有物品组。
        /// </summary>
        /// <param name="results">所有物品组。</param>
        public void GetAllItemGroups(List<IItemGroup> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, ItemGroup> itemGroup in m_ItemGroups)
            {
                results.Add(itemGroup.Value);
            }
        }


        /// <summary>
        /// 增加物品组。
        /// </summary>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <param name="instanceAutoReleaseInterval">物品实例对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="instanceCapacity">物品实例对象池容量。</param>
        /// <param name="instanceExpireTime">物品实例对象池对象过期秒数。</param>
        /// <param name="instancePriority">物品实例对象池的优先级。</param>
        /// <param name="itemGroupHelper">物品组辅助器。</param>
        /// <returns>是否增加物品组成功。</returns>
        public bool AddItemGroup(string itemGroupName, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority, IItemGroupHelper itemGroupHelper)
        {
            if (string.IsNullOrEmpty(itemGroupName))
            {
                throw new GameFrameworkException("Item group name is invalid.");
            }

            if (itemGroupHelper == null)
            {
                throw new GameFrameworkException("Item group helper is invalid.");
            }

            if (m_ObjectPoolManager == null)
            {
                throw new GameFrameworkException("You must set object pool manager first.");
            }

            if (HasItemGroup(itemGroupName))
            {
                return false;
            }

            m_ItemGroups.Add(itemGroupName, new ItemGroup(itemGroupName, instanceAutoReleaseInterval, instanceCapacity, instanceExpireTime, instancePriority, itemGroupHelper, m_ObjectPoolManager));

            return true;
        }

        /// <summary>
        /// 是否存在物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <returns>是否存在物品。</returns>
        public bool HasItem(int itemId)
        {
            return m_ItemInfos.ContainsKey(itemId);
        }

        /// <summary>
        /// 是否存在物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <returns>是否存在物品。</returns>
        public bool HasItem(string itemAssetName)
        {
            if (string.IsNullOrEmpty(itemAssetName))
            {
                throw new GameFrameworkException("Item asset name is invalid.");
            }

            foreach (KeyValuePair<int, ItemInfo> itemInfo in m_ItemInfos)
            {
                if (itemInfo.Value.Item.ItemAssetName == itemAssetName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <returns>要获取的物品。</returns>
        public IItem GetItem(int itemId)
        {
            ItemInfo itemInfo = GetItemInfo(itemId);
            if (itemInfo == null)
            {
                return null;
            }

            return itemInfo.Item;
        }

        /// <summary>
        /// 获取物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <returns>要获取的物品。</returns>
        public IItem GetItem(string itemAssetName)
        {
            if (string.IsNullOrEmpty(itemAssetName))
            {
                throw new GameFrameworkException("Item asset name is invalid.");
            }

            foreach (KeyValuePair<int, ItemInfo> itemInfo in m_ItemInfos)
            {
                if (itemInfo.Value.Item.ItemAssetName == itemAssetName)
                {
                    return itemInfo.Value.Item;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <returns>要获取的物品。</returns>
        public IItem[] GetItems(string itemAssetName)
        {
            if (string.IsNullOrEmpty(itemAssetName))
            {
                throw new GameFrameworkException("Item asset name is invalid.");
            }

            List<IItem> results = new List<IItem>();
            foreach (KeyValuePair<int, ItemInfo> itemInfo in m_ItemInfos)
            {
                if (itemInfo.Value.Item.ItemAssetName == itemAssetName)
                {
                    results.Add(itemInfo.Value.Item);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="results">要获取的物品。</param>
        public void GetItems(string itemAssetName, List<IItem> results)
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
            foreach (KeyValuePair<int, ItemInfo> itemInfo in m_ItemInfos)
            {
                if (itemInfo.Value.Item.ItemAssetName == itemAssetName)
                {
                    results.Add(itemInfo.Value.Item);
                }
            }
        }

        /// <summary>
        /// 获取所有已加载的物品。
        /// </summary>
        /// <returns>所有已加载的物品。</returns>
        public IItem[] GetAllLoadedItems()
        {
            int index = 0;
            IItem[] results = new IItem[m_ItemInfos.Count];
            foreach (KeyValuePair<int, ItemInfo> itemInfo in m_ItemInfos)
            {
                results[index++] = itemInfo.Value.Item;
            }

            return results;
        }

        /// <summary>
        /// 获取所有已加载的物品。
        /// </summary>
        /// <param name="results">所有已加载的物品。</param>
        public void GetAllLoadedItems(List<IItem> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<int, ItemInfo> itemInfo in m_ItemInfos)
            {
                results.Add(itemInfo.Value.Item);
            }
        }

        /// <summary>
        /// 获取所有正在加载物品的编号。
        /// </summary>
        /// <returns>所有正在加载物品的编号。</returns>
        public int[] GetAllLoadingItemIds()
        {
            int index = 0;
            int[] results = new int[m_ItemsBeingLoaded.Count];
            foreach (KeyValuePair<int, int> itemBeingLoaded in m_ItemsBeingLoaded)
            {
                results[index++] = itemBeingLoaded.Key;
            }

            return results;
        }

        /// <summary>
        /// 获取所有正在加载物品的编号。
        /// </summary>
        /// <param name="results">所有正在加载物品的编号。</param>
        public void GetAllLoadingItemIds(List<int> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<int, int> itemBeingLoaded in m_ItemsBeingLoaded)
            {
                results.Add(itemBeingLoaded.Key);
            }
        }

        /// <summary>
        /// 是否正在加载物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <returns>是否正在加载物品。</returns>
        public bool IsLoadingItem(int itemId)
        {
            return m_ItemsBeingLoaded.ContainsKey(itemId);
        }

        /// <summary>
        /// 是否是合法的物品。
        /// </summary>
        /// <param name="item">物品。</param>
        /// <returns>物品是否合法。</returns>
        public bool IsValidItem(IItem item)
        {
            if (item == null)
            {
                return false;
            }

            return HasItem(item.Id);
        }

        /// <summary>
        /// 显示物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="itemGroupName">物品组名称。</param>
        public void ShowItem(int itemId, string itemAssetName, string itemGroupName)
        {
            ShowItem(itemId, itemAssetName, itemGroupName, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 显示物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <param name="priority">加载物品资源的优先级。</param>
        public void ShowItem(int itemId, string itemAssetName, string itemGroupName, int priority)
        {
            ShowItem(itemId, itemAssetName, itemGroupName, priority, null);
        }

        /// <summary>
        /// 显示物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ShowItem(int itemId, string itemAssetName, string itemGroupName, object userData)
        {
            ShowItem(itemId, itemAssetName, itemGroupName, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 显示物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <param name="priority">加载物品资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ShowItem(int itemId, string itemAssetName, string itemGroupName, int priority, object userData)
        {
            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (m_ItemHelper == null)
            {
                throw new GameFrameworkException("You must set item helper first.");
            }

            if (string.IsNullOrEmpty(itemAssetName))
            {
                throw new GameFrameworkException("Item asset name is invalid.");
            }

            if (string.IsNullOrEmpty(itemGroupName))
            {
                throw new GameFrameworkException("Item group name is invalid.");
            }

            if (HasItem(itemId))
            {
                throw new GameFrameworkException(Utility.Text.Format("Item id '{0}' is already exist.", itemId.ToString()));
            }

            if (IsLoadingItem(itemId))
            {
                throw new GameFrameworkException(Utility.Text.Format("Item '{0}' is already being loaded.", itemId.ToString()));
            }

            ItemGroup itemGroup = (ItemGroup)GetItemGroup(itemGroupName);
            if (itemGroup == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Item group '{0}' is not exist.", itemGroupName));
            }

            ItemInstanceObject itemInstanceObject = itemGroup.SpawnItemInstanceObject(itemAssetName);
            if (itemInstanceObject == null)
            {
                int serialId = ++m_Serial;
                m_ItemsBeingLoaded.Add(itemId, serialId);
                m_ResourceManager.LoadAsset(itemAssetName, priority, m_LoadAssetCallbacks, ShowItemInfo.Create(serialId, itemId, itemGroup, userData));
                return;
            }

            InternalShowItem(itemId, itemAssetName, itemGroup, itemInstanceObject.Target, false, 0f, userData);
        }

        /// <summary>
        /// 隐藏物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        public void HideItem(int itemId)
        {
            HideItem(itemId, null);
        }

        /// <summary>
        /// 隐藏物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void HideItem(int itemId, object userData)
        {
            if (IsLoadingItem(itemId))
            {
                m_ItemsToReleaseOnLoad.Add(m_ItemsBeingLoaded[itemId]);
                m_ItemsBeingLoaded.Remove(itemId);
                return;
            }

            ItemInfo itemInfo = GetItemInfo(itemId);
            if (itemInfo == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not find item '{0}'.", itemId.ToString()));
            }

            InternalHideItem(itemInfo, userData);
        }

        /// <summary>
        /// 隐藏物品。
        /// </summary>
        /// <param name="item">物品。</param>
        public void HideItem(IItem item)
        {
            HideItem(item, null);
        }

        /// <summary>
        /// 隐藏物品。
        /// </summary>
        /// <param name="item">物品。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void HideItem(IItem item, object userData)
        {
            if (item == null)
            {
                throw new GameFrameworkException("Item is invalid.");
            }

            HideItem(item.Id, userData);
        }

        /// <summary>
        /// 隐藏所有已加载的物品。
        /// </summary>
        public void HideAllLoadedItems()
        {
            HideAllLoadedItems(null);
        }

        /// <summary>
        /// 隐藏所有已加载的物品。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void HideAllLoadedItems(object userData)
        {
            while (m_ItemInfos.Count > 0)
            {
                foreach (KeyValuePair<int, ItemInfo> itemInfo in m_ItemInfos)
                {
                    InternalHideItem(itemInfo.Value, userData);
                    break;
                }
            }
        }

        /// <summary>
        /// 隐藏所有正在加载的物品。
        /// </summary>
        public void HideAllLoadingItems()
        {
            foreach (KeyValuePair<int, int> itemBeingLoaded in m_ItemsBeingLoaded)
            {
                m_ItemsToReleaseOnLoad.Add(itemBeingLoaded.Value);
            }

            m_ItemsBeingLoaded.Clear();
        }

        /// <summary>
        /// 获取实体信息。
        /// </summary>
        /// <param name="itemId">实体编号。</param>
        /// <returns>实体信息。</returns>
        private ItemInfo GetItemInfo(int itemId)
        {
            ItemInfo itemInfo = null;
            if (m_ItemInfos.TryGetValue(itemId, out itemInfo))
            {
                return itemInfo;
            }

            return null;
        }

        private void InternalShowItem(int itemId, string itemAssetName, ItemGroup itemGroup, object itemInstance, bool isNewInstance, float duration, object userData)
        {
            try
            {
                IItem item = m_ItemHelper.CreateItem(itemInstance, itemGroup, userData);
                if (item == null)
                {
                    throw new GameFrameworkException("Can not create item in helper.");
                }

                ItemInfo itemInfo = ItemInfo.Create(item);
                m_ItemInfos.Add(itemId, itemInfo);
                itemInfo.Status = ItemStatus.WillInit;
                item.OnInit(itemId, itemAssetName, itemGroup, isNewInstance, userData);
                itemInfo.Status = ItemStatus.Inited;
                itemGroup.AddItem(item);
                itemInfo.Status = ItemStatus.WillShow;
                item.OnShow(userData);
                itemInfo.Status = ItemStatus.Showed;

                if (m_ShowItemSuccessEventHandler != null)
                {
                    ShowItemSuccessEventArgs showItemSuccessEventArgs = ShowItemSuccessEventArgs.Create(item, duration, userData);
                    m_ShowItemSuccessEventHandler(this, showItemSuccessEventArgs);
                    ReferencePool.Release(showItemSuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (m_ShowItemFailureEventHandler != null)
                {
                    ShowItemFailureEventArgs showItemFailureEventArgs = ShowItemFailureEventArgs.Create(itemId, itemAssetName, itemGroup.Name, exception.ToString(), userData);
                    m_ShowItemFailureEventHandler(this, showItemFailureEventArgs);
                    ReferencePool.Release(showItemFailureEventArgs);
                    return;
                }

                throw;
            }
        }

        private void InternalHideItem(ItemInfo itemInfo, object userData)
        {
            IItem item = itemInfo.Item;

            if (itemInfo.Status == ItemStatus.Hidden)
            {
                return;
            }

            itemInfo.Status = ItemStatus.WillHide;
            item.OnHide(m_IsShutdown, userData);
            itemInfo.Status = ItemStatus.Hidden;

            ItemGroup itemGroup = (ItemGroup)item.ItemGroup;
            if (itemGroup == null)
            {
                throw new GameFrameworkException("Item group is invalid.");
            }

            itemGroup.RemoveItem(item);
            if (!m_ItemInfos.Remove(item.Id))
            {
                throw new GameFrameworkException("Item info is unmanaged.");
            }

            if (m_HideItemCompleteEventHandler != null)
            {
                HideItemCompleteEventArgs hideItemCompleteEventArgs = HideItemCompleteEventArgs.Create(item.Id, item.ItemAssetName, itemGroup, userData);
                m_HideItemCompleteEventHandler(this, hideItemCompleteEventArgs);
                ReferencePool.Release(hideItemCompleteEventArgs);
            }

            m_RecycleQueue.Enqueue(itemInfo);
        }

        private void LoadAssetSuccessCallback(string ItemAssetName, object ItemAsset, float duration, object userData)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)userData;
            if (showItemInfo == null)
            {
                throw new GameFrameworkException("Show Item info is invalid.");
            }

            if (m_ItemsToReleaseOnLoad.Contains(showItemInfo.SerialId))
            {
                m_ItemsToReleaseOnLoad.Remove(showItemInfo.SerialId);
                ReferencePool.Release(showItemInfo);
                m_ItemHelper.ReleaseItem(ItemAsset, null);
                return;
            }

            m_ItemsBeingLoaded.Remove(showItemInfo.ItemId);
            ItemInstanceObject itemInstanceObject = ItemInstanceObject.Create(ItemAssetName, ItemAsset, m_ItemHelper.InstantiateItem(ItemAsset), m_ItemHelper);
            showItemInfo.ItemGroup.RegisterItemInstanceObject(itemInstanceObject, true);

            InternalShowItem(showItemInfo.ItemId, ItemAssetName, showItemInfo.ItemGroup, itemInstanceObject.Target, true, duration, showItemInfo.UserData);
            ReferencePool.Release(showItemInfo);
        }

        private void LoadAssetFailureCallback(string ItemAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)userData;
            if (showItemInfo == null)
            {
                throw new GameFrameworkException("Show Item info is invalid.");
            }

            if (m_ItemsToReleaseOnLoad.Contains(showItemInfo.SerialId))
            {
                m_ItemsToReleaseOnLoad.Remove(showItemInfo.SerialId);
                return;
            }

            m_ItemsBeingLoaded.Remove(showItemInfo.ItemId);
            string appendErrorMessage = Utility.Text.Format("Load Item failure, asset name '{0}', status '{1}', error message '{2}'.", ItemAssetName, status.ToString(), errorMessage);
            if (m_ShowItemFailureEventHandler != null)
            {
                ShowItemFailureEventArgs showItemFailureEventArgs = ShowItemFailureEventArgs.Create(showItemInfo.ItemId, ItemAssetName, showItemInfo.ItemGroup.Name, appendErrorMessage, showItemInfo.UserData);
                m_ShowItemFailureEventHandler(this, showItemFailureEventArgs);
                ReferencePool.Release(showItemFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string ItemAssetName, float progress, object userData)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)userData;
            if (showItemInfo == null)
            {
                throw new GameFrameworkException("Show Item info is invalid.");
            }

            if (m_ShowItemUpdateEventHandler != null)
            {
                ShowItemUpdateEventArgs showItemUpdateEventArgs = ShowItemUpdateEventArgs.Create(showItemInfo.ItemId, ItemAssetName, showItemInfo.ItemGroup.Name, progress, showItemInfo.UserData);
                m_ShowItemUpdateEventHandler(this, showItemUpdateEventArgs);
                ReferencePool.Release(showItemUpdateEventArgs);
            }
        }

        private void LoadAssetDependencyAssetCallback(string ItemAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)userData;
            if (showItemInfo == null)
            {
                throw new GameFrameworkException("Show Item info is invalid.");
            }

            if (m_ShowItemDependencyAssetEventHandler != null)
            {
                ShowItemDependencyAssetEventArgs showItemDependencyAssetEventArgs = ShowItemDependencyAssetEventArgs.Create(showItemInfo.ItemId, ItemAssetName, showItemInfo.ItemGroup.Name, dependencyAssetName, loadedCount, totalCount, showItemInfo.UserData);
                m_ShowItemDependencyAssetEventHandler(this, showItemDependencyAssetEventArgs);
                ReferencePool.Release(showItemDependencyAssetEventArgs);
            }
        }
    }
}

