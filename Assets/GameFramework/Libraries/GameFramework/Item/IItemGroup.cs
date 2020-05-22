using System.Collections.Generic;

namespace GameFramework.Item
{
    public interface IItemGroup
    {
        /// <summary>
        /// 获取物品组名称。
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取物品组中物品数量。
        /// </summary>
        int ItemCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置物品组实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float InstanceAutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置物品组实例对象池的容量。
        /// </summary>
        int InstanceCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置物品组实例对象池对象过期秒数。
        /// </summary>
        float InstanceExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置物品组实例对象池的优先级。
        /// </summary>
        int InstancePriority
        {
            get;
            set;
        }

        /// <summary>
        /// 获取物品组辅助器。
        /// </summary>
        IItemGroupHelper Helper
        {
            get;
        }

        /// <summary>
        /// 物品组中是否存在物品。
        /// </summary>
        /// <param name="itemId">物品序列编号。</param>
        /// <returns>物品组中是否存在物品。</returns>
        bool HasItem(int itemId);

        /// <summary>
        /// 物品组中是否存在物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <returns>物品组中是否存在物品。</returns>
        bool HasItem(string itemAssetName);

        /// <summary>
        /// 从物品组中获取物品。
        /// </summary>
        /// <param name="itemId">物品序列编号。</param>
        /// <returns>要获取的物品。</returns>
        IItem GetItem(int itemId);

        /// <summary>
        /// 从物品组中获取物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <returns>要获取的物品。</returns>
        IItem GetItem(string itemAssetName);

        /// <summary>
        /// 从物品组中获取物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <returns>要获取的物品。</returns>
        IItem[] GetItems(string itemAssetName);

        /// <summary>
        /// 从物品组中获取物品。
        /// </summary>
        /// <param name="itemAssetName">物品资源名称。</param>
        /// <param name="results">要获取的物品。</param>
        void GetItems(string itemAssetName, List<IItem> results);

        /// <summary>
        /// 从物品组中获取所有物品。
        /// </summary>
        /// <returns>物品组中的所有物品。</returns>
        IItem[] GetAllItems();

        /// <summary>
        /// 从物品组中获取所有物品。
        /// </summary>
        /// <param name="results">物品组中的所有物品。</param>
        void GetAllItems(List<IItem> results);

        /// <summary>
        /// 设置物品实例是否被加锁。
        /// </summary>
        /// <param name="itemInstance">物品实例。</param>
        /// <param name="locked">物品实例是否被加锁。</param>
        void SetItemInstanceLocked(object itemInstance, bool locked);

        /// <summary>
        /// 设置物品实例的优先级。
        /// </summary>
        /// <param name="itemInstance">物品实例。</param>
        /// <param name="priority">物品实例优先级。</param>
        void SetItemInstancePriority(object itemInstance, int priority);
    }
}

