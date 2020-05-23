using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Item
{
    public interface IItemManager
    {

        /// <summary>
        /// 获取物品数量。
        /// </summary>
        int ItemCount
        {
            get;
        }

        /// <summary>
        /// 获取物品组数量。
        /// </summary>
        int ItemGroupCount
        {
            get;
        }

        /// <summary>
        /// 显示物品成功事件。
        /// </summary>
        event EventHandler<ShowItemSuccessEventArgs> ShowItemSuccess;

        /// <summary>
        /// 显示物品失败事件。
        /// </summary>
        event EventHandler<ShowItemFailureEventArgs> ShowItemFailure;

        /// <summary>
        /// 显示物品更新事件。
        /// </summary>
        event EventHandler<ShowItemUpdateEventArgs> ShowItemUpdate;

        /// <summary>
        /// 显示物品时加载依赖资源事件。
        /// </summary>
        event EventHandler<ShowItemDependencyAssetEventArgs> ShowItemDependencyAsset;

        /// <summary>
        /// 隐藏物品完成事件。
        /// </summary>
        event EventHandler<HideItemCompleteEventArgs> HideItemComplete;

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        void SetResourceManager(IResourceManager resourceManager);

        /// <summary>
        /// 设置物品辅助器。
        /// </summary>
        /// <param name="itemHelper">物品辅助器。</param>
        void SetItemHelper(IItemHelper itemHelper);

        /// <summary>
        /// 是否存在物品组。
        /// </summary>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <returns>是否存在物品组。</returns>
        bool HasItemGroup(string itemGroupName);

        /// <summary>
        /// 获取物品组。
        /// </summary>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <returns>要获取的物品组。</returns>
        IItemGroup GetItemGroup(string itemGroupName);

        /// <summary>
        /// 获取所有物品组。
        /// </summary>
        /// <returns>所有物品组。</returns>
        IItemGroup[] GetAllItemGroups();

        /// <summary>
        /// 获取所有物品组。
        /// </summary>
        /// <param name="results">所有物品组。</param>
        void GetAllItemGroups(List<IItemGroup> results);

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
        bool AddItemGroup(string itemGroupName, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority, IItemGroupHelper itemGroupHelper);

        /// <summary>
        /// 是否存在物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <returns>是否存在物品。</returns>
        bool HasItem(int itemId);

        /// <summary>
        /// 是否存在物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <returns>是否存在物品。</returns>
        bool HasItem(string itemAssetName);

        /// <summary>
        /// 获取物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <returns>要获取的物品。</returns>
        IItem GetItem(int itemId);

        /// <summary>
        /// 获取物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <returns>要获取的物品。</returns>
        IItem GetItem(string itemAssetName);

        /// <summary>
        /// 获取物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <returns>要获取的物品。</returns>
        IItem[] GetItems(string itemAssetName);

        /// <summary>
        /// 获取物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="results">要获取的物品。</param>
        void GetItems(string itemAssetName, List<IItem> results);

        /// <summary>
        /// 获取所有已加载的物品。
        /// </summary>
        /// <returns>所有已加载的物品。</returns>
        IItem[] GetAllLoadedItems();

        /// <summary>
        /// 获取所有已加载的物品。
        /// </summary>
        /// <param name="results">所有已加载的物品。</param>
        void GetAllLoadedItems(List<IItem> results);

        /// <summary>
        /// 获取所有正在加载物品的编号。
        /// </summary>
        /// <returns>所有正在加载物品的编号。</returns>
        int[] GetAllLoadingItemIds();

        /// <summary>
        /// 获取所有正在加载物品的编号。
        /// </summary>
        /// <param name="results">所有正在加载物品的编号。</param>
        void GetAllLoadingItemIds(List<int> results);

        /// <summary>
        /// 是否正在加载物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <returns>是否正在加载物品。</returns>
        bool IsLoadingItem(int itemId);

        /// <summary>
        /// 是否是合法的物品。
        /// </summary>
        /// <param name="item">物品。</param>
        /// <returns>物品是否合法。</returns>
        bool IsValidItem(IItem item);

        /// <summary>
        /// 显示物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="itemGroupName">物品组名称。</param>
        void ShowItem(int itemId, string itemAssetName, string itemGroupName);

        /// <summary>
        /// 显示物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <param name="priority">加载物品资源的优先级。</param>
        void ShowItem(int itemId, string itemAssetName, string itemGroupName, int priority);

        /// <summary>
        /// 显示物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        void ShowItem(int itemId, string itemAssetName, string itemGroupName, object userData);

        /// <summary>
        /// 显示物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="itemGroupName">物品组名称。</param>
        /// <param name="priority">加载物品资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        void ShowItem(int itemId, string itemAssetName, string itemGroupName, int priority, object userData);

        /// <summary>
        /// 隐藏物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        void HideItem(int itemId);

        /// <summary>
        /// 隐藏物品。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        void HideItem(int itemId, object userData);

        /// <summary>
        /// 隐藏物品。
        /// </summary>
        /// <param name="item">物品。</param>
        void HideItem(IItem item);

        /// <summary>
        /// 隐藏物品。
        /// </summary>
        /// <param name="item">物品。</param>
        /// <param name="userData">用户自定义数据。</param>
        void HideItem(IItem item, object userData);

        /// <summary>
        /// 隐藏所有已加载的物品。
        /// </summary>
        void HideAllLoadedItems();

        /// <summary>
        /// 隐藏所有已加载的物品。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        void HideAllLoadedItems(object userData);

        /// <summary>
        /// 隐藏所有正在加载的物品。
        /// </summary>
        void HideAllLoadingItems();
    }
}


