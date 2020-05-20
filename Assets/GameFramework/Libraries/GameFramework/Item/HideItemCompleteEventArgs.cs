namespace GameFramework.Item
{
    /// <summary>
    /// 隐藏物品完成事件。
    /// </summary>
    public sealed class HideItemCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化隐藏物品完成事件的新实例。
        /// </summary>
        public HideItemCompleteEventArgs()
        {
            ItemId = 0;
            ItemAssetName = null;
            ItemGroup = null;
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
        /// 获取物品所属的物品组。
        /// </summary>
        public IItemGroup ItemGroup
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
        /// 创建隐藏物品完成事件。
        /// </summary>
        /// <param name="ItemId">物品编号。</param>
        /// <param name="ItemAssetName">物品资源名称。</param>
        /// <param name="ItemGroup">物品所属的物品组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的隐藏物品完成事件。</returns>
        public static HideItemCompleteEventArgs Create(int ItemId, string ItemAssetName, IItemGroup ItemGroup, object userData)
        {
            HideItemCompleteEventArgs hideItemCompleteEventArgs = ReferencePool.Acquire<HideItemCompleteEventArgs>();
            hideItemCompleteEventArgs.ItemId = ItemId;
            hideItemCompleteEventArgs.ItemAssetName = ItemAssetName;
            hideItemCompleteEventArgs.ItemGroup = ItemGroup;
            hideItemCompleteEventArgs.UserData = userData;
            return hideItemCompleteEventArgs;
        }

        /// <summary>
        /// 清理隐藏物品完成事件。
        /// </summary>
        public override void Clear()
        {
            ItemId = 0;
            ItemAssetName = null;
            ItemGroup = null;
            UserData = null;
        }
    }
}
