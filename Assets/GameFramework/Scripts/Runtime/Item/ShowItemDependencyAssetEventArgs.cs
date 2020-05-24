using GameFramework;
using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 显示物体时加载依赖资源事件。
    /// </summary>
    public sealed class ShowItemDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示物体时加载依赖资源事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ShowItemDependencyAssetEventArgs).GetHashCode();

        /// <summary>
        /// 初始化显示物体时加载依赖资源事件的新实例。
        /// </summary>
        public ShowItemDependencyAssetEventArgs()
        {
            ItemId = 0;
            ItemLogicType = null;
            ItemAssetName = null;
            ItemGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取显示物体时加载依赖资源事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取物体编号。
        /// </summary>
        public int ItemId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物体逻辑类型。
        /// </summary>
        public Type ItemLogicType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物体资源名称。
        /// </summary>
        public string ItemAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物体组名称。
        /// </summary>
        public string ItemGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建显示物体时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的显示物体时加载依赖资源事件。</returns>
        public static ShowItemDependencyAssetEventArgs Create(GameFramework.Item.ShowItemDependencyAssetEventArgs e)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)e.UserData;
            ShowItemDependencyAssetEventArgs showItemDependencyAssetEventArgs = ReferencePool.Acquire<ShowItemDependencyAssetEventArgs>();
            showItemDependencyAssetEventArgs.ItemId = e.ItemId;
            showItemDependencyAssetEventArgs.ItemLogicType = showItemInfo.ItemLogicType;
            showItemDependencyAssetEventArgs.ItemAssetName = e.ItemAssetName;
            showItemDependencyAssetEventArgs.ItemGroupName = e.ItemGroupName;
            showItemDependencyAssetEventArgs.DependencyAssetName = e.DependencyAssetName;
            showItemDependencyAssetEventArgs.LoadedCount = e.LoadedCount;
            showItemDependencyAssetEventArgs.TotalCount = e.TotalCount;
            showItemDependencyAssetEventArgs.UserData = showItemInfo.UserData;
            return showItemDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理显示物体时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            ItemId = 0;
            ItemLogicType = null;
            ItemAssetName = null;
            ItemGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
