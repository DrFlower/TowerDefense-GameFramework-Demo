namespace GameFramework.Item
{
    /// <summary>
    /// 显示物品时加载依赖资源事件。
    /// </summary>
    public sealed class ShowItemDependencyAssetEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化显示物品时加载依赖资源事件的新实例。
        /// </summary>
        public ShowItemDependencyAssetEventArgs()
        {
            ItemId = 0;
            ItemAssetName = null;
            ItemGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取物品编号。
        /// </summary>
        public int ItemId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物品资源名称。
        /// </summary>
        public string ItemAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物品组名称。
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
        /// 创建显示物品时加载依赖资源事件。
        /// </summary>
        /// <param name="ItemId">物品编号。</param>
        /// <param name="ItemAssetName">物品资源名称。</param>
        /// <param name="ItemGroupName">物品组名称。</param>
        /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的显示物品时加载依赖资源事件。</returns>
        public static ShowItemDependencyAssetEventArgs Create(int ItemId, string ItemAssetName, string ItemGroupName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            ShowItemDependencyAssetEventArgs showItemDependencyAssetEventArgs = ReferencePool.Acquire<ShowItemDependencyAssetEventArgs>();
            showItemDependencyAssetEventArgs.ItemId = ItemId;
            showItemDependencyAssetEventArgs.ItemAssetName = ItemAssetName;
            showItemDependencyAssetEventArgs.ItemGroupName = ItemGroupName;
            showItemDependencyAssetEventArgs.DependencyAssetName = dependencyAssetName;
            showItemDependencyAssetEventArgs.LoadedCount = loadedCount;
            showItemDependencyAssetEventArgs.TotalCount = totalCount;
            showItemDependencyAssetEventArgs.UserData = userData;
            return showItemDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理显示物品时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            ItemId = 0;
            ItemAssetName = null;
            ItemGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
