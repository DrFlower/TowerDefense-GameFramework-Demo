namespace GameFramework.Item
{
    /// <summary>
    /// 显示物品更新事件。
    /// </summary>
    public sealed class ShowItemUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化显示物品更新事件的新实例。
        /// </summary>
        public ShowItemUpdateEventArgs()
        {
            ItemId = 0;
            ItemAssetName = null;
            ItemGroupName = null;
            Progress = 0f;
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
        /// 获取显示物品进度。
        /// </summary>
        public float Progress
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
        /// 创建显示物品更新事件。
        /// </summary>
        /// <param name="ItemId">物品编号。</param>
        /// <param name="ItemAssetName">物品资源名称。</param>
        /// <param name="ItemGroupName">物品组名称。</param>
        /// <param name="progress">显示物品进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的显示物品更新事件。</returns>
        public static ShowItemUpdateEventArgs Create(int ItemId, string ItemAssetName, string ItemGroupName, float progress, object userData)
        {
            ShowItemUpdateEventArgs showItemUpdateEventArgs = ReferencePool.Acquire<ShowItemUpdateEventArgs>();
            showItemUpdateEventArgs.ItemId = ItemId;
            showItemUpdateEventArgs.ItemAssetName = ItemAssetName;
            showItemUpdateEventArgs.ItemGroupName = ItemGroupName;
            showItemUpdateEventArgs.Progress = progress;
            showItemUpdateEventArgs.UserData = userData;
            return showItemUpdateEventArgs;
        }

        /// <summary>
        /// 清理显示物品更新事件。
        /// </summary>
        public override void Clear()
        {
            ItemId = 0;
            ItemAssetName = null;
            ItemGroupName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}
