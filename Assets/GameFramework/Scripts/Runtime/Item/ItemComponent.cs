using GameFramework;
using GameFramework.Item;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 物体组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Item")]
    public sealed partial class ItemComponent : GameFrameworkComponent
    {
        private const int DefaultPriority = 0;

        private IItemManager m_ItemManager = null;
        private EventComponent m_EventComponent = null;

        private readonly List<IItem> m_InternalItemResults = new List<IItem>();

        [SerializeField]
        private bool m_EnableShowItemUpdateEvent = false;

        [SerializeField]
        private bool m_EnableShowItemDependencyAssetEvent = false;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private string m_ItemHelperTypeName = "UnityGameFramework.Runtime.DefaultItemHelper";

        [SerializeField]
        private ItemHelperBase m_CustomItemHelper = null;

        [SerializeField]
        private string m_ItemGroupHelperTypeName = "UnityGameFramework.Runtime.DefaultItemGroupHelper";

        [SerializeField]
        private ItemGroupHelperBase m_CustomItemGroupHelper = null;

        [SerializeField]
        private ItemGroup[] m_ItemGroups = null;

        /// <summary>
        /// 获取物体数量。
        /// </summary>
        public int ItemCount
        {
            get
            {
                return m_ItemManager.ItemCount;
            }
        }

        /// <summary>
        /// 获取物体组数量。
        /// </summary>
        public int ItemGroupCount
        {
            get
            {
                return m_ItemManager.ItemGroupCount;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_ItemManager = GameFrameworkEntry.GetModule<IItemManager>();
            if (m_ItemManager == null)
            {
                Log.Fatal("Item manager is invalid.");
                return;
            }

            m_ItemManager.ShowItemSuccess += OnShowItemSuccess;
            m_ItemManager.ShowItemFailure += OnShowItemFailure;

            if (m_EnableShowItemUpdateEvent)
            {
                m_ItemManager.ShowItemUpdate += OnShowItemUpdate;
            }

            if (m_EnableShowItemDependencyAssetEvent)
            {
                m_ItemManager.ShowItemDependencyAsset += OnShowItemDependencyAsset;
            }

            m_ItemManager.HideItemComplete += OnHideItemComplete;
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                m_ItemManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                m_ItemManager.SetResourceManager(GameFrameworkEntry.GetModule<IResourceManager>());
            }

            m_ItemManager.SetObjectPoolManager(GameFrameworkEntry.GetModule<IObjectPoolManager>());

            ItemHelperBase itemHelper = Helper.CreateHelper(m_ItemHelperTypeName, m_CustomItemHelper);
            if (itemHelper == null)
            {
                Log.Error("Can not create item helper.");
                return;
            }

            itemHelper.name = "Item Helper";
            Transform transform = itemHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_ItemManager.SetItemHelper(itemHelper);

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = new GameObject("Item Instances").transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_ItemGroups.Length; i++)
            {
                if (!AddItemGroup(m_ItemGroups[i].Name, m_ItemGroups[i].InstanceAutoReleaseInterval, m_ItemGroups[i].InstanceCapacity, m_ItemGroups[i].InstanceExpireTime, m_ItemGroups[i].InstancePriority))
                {
                    Log.Warning("Add item group '{0}' failure.", m_ItemGroups[i].Name);
                    continue;
                }
            }
        }

        /// <summary>
        /// 是否存在物体组。
        /// </summary>
        /// <param name="itemGroupName">物体组名称。</param>
        /// <returns>是否存在物体组。</returns>
        public bool HasItemGroup(string itemGroupName)
        {
            return m_ItemManager.HasItemGroup(itemGroupName);
        }

        /// <summary>
        /// 获取物体组。
        /// </summary>
        /// <param name="itemGroupName">物体组名称。</param>
        /// <returns>要获取的物体组。</returns>
        public IItemGroup GetItemGroup(string itemGroupName)
        {
            return m_ItemManager.GetItemGroup(itemGroupName);
        }

        /// <summary>
        /// 获取所有物体组。
        /// </summary>
        /// <returns>所有物体组。</returns>
        public IItemGroup[] GetAllItemGroups()
        {
            return m_ItemManager.GetAllItemGroups();
        }

        /// <summary>
        /// 获取所有物体组。
        /// </summary>
        /// <param name="results">所有物体组。</param>
        public void GetAllItemGroups(List<IItemGroup> results)
        {
            m_ItemManager.GetAllItemGroups(results);
        }

        /// <summary>
        /// 增加物体组。
        /// </summary>
        /// <param name="itemGroupName">物体组名称。</param>
        /// <param name="instanceAutoReleaseInterval">物体实例对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="instanceCapacity">物体实例对象池容量。</param>
        /// <param name="instanceExpireTime">物体实例对象池对象过期秒数。</param>
        /// <param name="instancePriority">物体实例对象池的优先级。</param>
        /// <returns>是否增加物体组成功。</returns>
        public bool AddItemGroup(string itemGroupName, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority)
        {
            if (m_ItemManager.HasItemGroup(itemGroupName))
            {
                return false;
            }

            ItemGroupHelperBase itemGroupHelper = Helper.CreateHelper(m_ItemGroupHelperTypeName, m_CustomItemGroupHelper, ItemGroupCount);
            if (itemGroupHelper == null)
            {
                Log.Error("Can not create item group helper.");
                return false;
            }

            itemGroupHelper.name = Utility.Text.Format("Item Group - {0}", itemGroupName);
            Transform transform = itemGroupHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            return m_ItemManager.AddItemGroup(itemGroupName, instanceAutoReleaseInterval, instanceCapacity, instanceExpireTime, instancePriority, itemGroupHelper);
        }

        /// <summary>
        /// 是否存在物体。
        /// </summary>
        /// <param name="itemId">物体编号。</param>
        /// <returns>是否存在物体。</returns>
        public bool HasItem(int itemId)
        {
            return m_ItemManager.HasItem(itemId);
        }

        /// <summary>
        /// 是否存在物体。
        /// </summary>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <returns>是否存在物体。</returns>
        public bool HasItem(string itemAssetName)
        {
            return m_ItemManager.HasItem(itemAssetName);
        }

        /// <summary>
        /// 获取物体。
        /// </summary>
        /// <param name="itemId">物体编号。</param>
        /// <returns>物体。</returns>
        public Item GetItem(int itemId)
        {
            return (Item)m_ItemManager.GetItem(itemId);
        }

        /// <summary>
        /// 获取物体。
        /// </summary>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <returns>要获取的物体。</returns>
        public Item GetItem(string itemAssetName)
        {
            return (Item)m_ItemManager.GetItem(itemAssetName);
        }

        /// <summary>
        /// 获取物体。
        /// </summary>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <returns>要获取的物体。</returns>
        public Item[] GetItems(string itemAssetName)
        {
            IItem[] items = m_ItemManager.GetItems(itemAssetName);
            Item[] itemImpls = new Item[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                itemImpls[i] = (Item)items[i];
            }

            return itemImpls;
        }

        /// <summary>
        /// 获取物体。
        /// </summary>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <param name="results">要获取的物体。</param>
        public void GetItems(string itemAssetName, List<Item> results)
        {
            if (results == null)
            {
                Log.Error("Results is invalid.");
                return;
            }

            results.Clear();
            m_ItemManager.GetItems(itemAssetName, m_InternalItemResults);
            foreach (IItem item in m_InternalItemResults)
            {
                results.Add((Item)item);
            }
        }

        /// <summary>
        /// 获取所有已加载的物体。
        /// </summary>
        /// <returns>所有已加载的物体。</returns>
        public Item[] GetAllLoadedItems()
        {
            IItem[] items = m_ItemManager.GetAllLoadedItems();
            Item[] itemImpls = new Item[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                itemImpls[i] = (Item)items[i];
            }

            return itemImpls;
        }

        /// <summary>
        /// 获取所有已加载的物体。
        /// </summary>
        /// <param name="results">所有已加载的物体。</param>
        public void GetAllLoadedItems(List<Item> results)
        {
            if (results == null)
            {
                Log.Error("Results is invalid.");
                return;
            }

            results.Clear();
            m_ItemManager.GetAllLoadedItems(m_InternalItemResults);
            foreach (IItem item in m_InternalItemResults)
            {
                results.Add((Item)item);
            }
        }

        /// <summary>
        /// 获取所有正在加载物体的编号。
        /// </summary>
        /// <returns>所有正在加载物体的编号。</returns>
        public int[] GetAllLoadingItemIds()
        {
            return m_ItemManager.GetAllLoadingItemIds();
        }

        /// <summary>
        /// 获取所有正在加载物体的编号。
        /// </summary>
        /// <param name="results">所有正在加载物体的编号。</param>
        public void GetAllLoadingItemIds(List<int> results)
        {
            m_ItemManager.GetAllLoadingItemIds(results);
        }

        /// <summary>
        /// 是否正在加载物体。
        /// </summary>
        /// <param name="itemId">物体编号。</param>
        /// <returns>是否正在加载物体。</returns>
        public bool IsLoadingItem(int itemId)
        {
            return m_ItemManager.IsLoadingItem(itemId);
        }

        /// <summary>
        /// 是否是合法的物体。
        /// </summary>
        /// <param name="item">物体。</param>
        /// <returns>物体是否合法。</returns>
        public bool IsValidItem(Item item)
        {
            return m_ItemManager.IsValidItem(item);
        }

        /// <summary>
        /// 显示物体。
        /// </summary>
        /// <typeparam name="T">物体逻辑类型。</typeparam>
        /// <param name="itemId">物体编号。</param>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <param name="itemGroupName">物体组名称。</param>
        public void ShowItem<T>(int itemId, string itemAssetName, string itemGroupName) where T : ItemLogic
        {
            ShowItem(itemId, typeof(T), itemAssetName, itemGroupName, DefaultPriority, null);
        }

        /// <summary>
        /// 显示物体。
        /// </summary>
        /// <param name="itemId">物体编号。</param>
        /// <param name="itemLogicType">物体逻辑类型。</param>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <param name="itemGroupName">物体组名称。</param>
        public void ShowItem(int itemId, Type itemLogicType, string itemAssetName, string itemGroupName)
        {
            ShowItem(itemId, itemLogicType, itemAssetName, itemGroupName, DefaultPriority, null);
        }

        /// <summary>
        /// 显示物体。
        /// </summary>
        /// <typeparam name="T">物体逻辑类型。</typeparam>
        /// <param name="itemId">物体编号。</param>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <param name="itemGroupName">物体组名称。</param>
        /// <param name="priority">加载物体资源的优先级。</param>
        public void ShowItem<T>(int itemId, string itemAssetName, string itemGroupName, int priority) where T : ItemLogic
        {
            ShowItem(itemId, typeof(T), itemAssetName, itemGroupName, priority, null);
        }

        /// <summary>
        /// 显示物体。
        /// </summary>
        /// <param name="itemId">物体编号。</param>
        /// <param name="itemLogicType">物体逻辑类型。</param>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <param name="itemGroupName">物体组名称。</param>
        /// <param name="priority">加载物体资源的优先级。</param>
        public void ShowItem(int itemId, Type itemLogicType, string itemAssetName, string itemGroupName, int priority)
        {
            ShowItem(itemId, itemLogicType, itemAssetName, itemGroupName, priority, null);
        }

        /// <summary>
        /// 显示物体。
        /// </summary>
        /// <typeparam name="T">物体逻辑类型。</typeparam>
        /// <param name="itemId">物体编号。</param>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <param name="itemGroupName">物体组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ShowItem<T>(int itemId, string itemAssetName, string itemGroupName, object userData) where T : ItemLogic
        {
            ShowItem(itemId, typeof(T), itemAssetName, itemGroupName, DefaultPriority, userData);
        }

        /// <summary>
        /// 显示物体。
        /// </summary>
        /// <param name="itemId">物体编号。</param>
        /// <param name="itemLogicType">物体逻辑类型。</param>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <param name="itemGroupName">物体组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ShowItem(int itemId, Type itemLogicType, string itemAssetName, string itemGroupName, object userData)
        {
            ShowItem(itemId, itemLogicType, itemAssetName, itemGroupName, DefaultPriority, userData);
        }

        /// <summary>
        /// 显示物体。
        /// </summary>
        /// <typeparam name="T">物体逻辑类型。</typeparam>
        /// <param name="itemId">物体编号。</param>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <param name="itemGroupName">物体组名称。</param>
        /// <param name="priority">加载物体资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ShowItem<T>(int itemId, string itemAssetName, string itemGroupName, int priority, object userData) where T : ItemLogic
        {
            ShowItem(itemId, typeof(T), itemAssetName, itemGroupName, priority, userData);
        }

        /// <summary>
        /// 显示物体。
        /// </summary>
        /// <param name="itemId">物体编号。</param>
        /// <param name="itemLogicType">物体逻辑类型。</param>
        /// <param name="itemAssetName">物体资源名称。</param>
        /// <param name="itemGroupName">物体组名称。</param>
        /// <param name="priority">加载物体资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ShowItem(int itemId, Type itemLogicType, string itemAssetName, string itemGroupName, int priority, object userData)
        {
            m_ItemManager.ShowItem(itemId, itemAssetName, itemGroupName, priority, ShowItemInfo.Create(itemLogicType, userData));
        }

        /// <summary>
        /// 隐藏物体。
        /// </summary>
        /// <param name="itemId">物体编号。</param>
        public void HideItem(int itemId)
        {
            m_ItemManager.HideItem(itemId);
        }

        /// <summary>
        /// 隐藏物体。
        /// </summary>
        /// <param name="itemId">物体编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void HideItem(int itemId, object userData)
        {
            m_ItemManager.HideItem(itemId, userData);
        }

        /// <summary>
        /// 隐藏物体。
        /// </summary>
        /// <param name="item">物体。</param>
        public void HideItem(Item item)
        {
            m_ItemManager.HideItem(item);
        }

        /// <summary>
        /// 隐藏物体。
        /// </summary>
        /// <param name="item">物体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void HideItem(Item item, object userData)
        {
            m_ItemManager.HideItem(item, userData);
        }

        /// <summary>
        /// 隐藏所有已加载的物体。
        /// </summary>
        public void HideAllLoadedItems()
        {
            m_ItemManager.HideAllLoadedItems();
        }

        /// <summary>
        /// 隐藏所有已加载的物体。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void HideAllLoadedItems(object userData)
        {
            m_ItemManager.HideAllLoadedItems(userData);
        }

        /// <summary>
        /// 隐藏所有正在加载的物体。
        /// </summary>
        public void HideAllLoadingItems()
        {
            m_ItemManager.HideAllLoadingItems();
        }

        /// <summary>
        /// 设置物体是否被加锁。
        /// </summary>
        /// <param name="item">物体。</param>
        /// <param name="locked">物体是否被加锁。</param>
        public void SetItemInstanceLocked(Item item, bool locked)
        {
            if (item == null)
            {
                Log.Warning("Item is invalid.");
                return;
            }

            IItemGroup itemGroup = item.ItemGroup;
            if (itemGroup == null)
            {
                Log.Warning("Item group is invalid.");
                return;
            }

            itemGroup.SetItemInstanceLocked(item.gameObject, locked);
        }

        /// <summary>
        /// 设置物体的优先级。
        /// </summary>
        /// <param name="item">物体。</param>
        /// <param name="priority">物体优先级。</param>
        public void SetInstancePriority(Item item, int priority)
        {
            if (item == null)
            {
                Log.Warning("Item is invalid.");
                return;
            }

            IItemGroup itemGroup = item.ItemGroup;
            if (itemGroup == null)
            {
                Log.Warning("Item group is invalid.");
                return;
            }

            itemGroup.SetItemInstancePriority(item.gameObject, priority);
        }

        private void OnShowItemSuccess(object sender, GameFramework.Item.ShowItemSuccessEventArgs e)
        {
            m_EventComponent.Fire(this, ShowItemSuccessEventArgs.Create(e));
        }

        private void OnShowItemFailure(object sender, GameFramework.Item.ShowItemFailureEventArgs e)
        {
            Log.Warning("Show item failure, item id '{0}', asset name '{1}', item group name '{2}', error message '{3}'.", e.ItemId.ToString(), e.ItemAssetName, e.ItemGroupName, e.ErrorMessage);
            m_EventComponent.Fire(this, ShowItemFailureEventArgs.Create(e));
        }

        private void OnShowItemUpdate(object sender, GameFramework.Item.ShowItemUpdateEventArgs e)
        {
            m_EventComponent.Fire(this, ShowItemUpdateEventArgs.Create(e));
        }

        private void OnShowItemDependencyAsset(object sender, GameFramework.Item.ShowItemDependencyAssetEventArgs e)
        {
            m_EventComponent.Fire(this, ShowItemDependencyAssetEventArgs.Create(e));
        }

        private void OnHideItemComplete(object sender, GameFramework.Item.HideItemCompleteEventArgs e)
        {
            m_EventComponent.Fire(this, HideItemCompleteEventArgs.Create(e));
        }
    }
}
