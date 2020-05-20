namespace GameFramework.Item
{
    /// <summary>
    /// 显示物品失败事件。
    /// </summary>
    public sealed class ShowItemFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化显示物品失败事件的新实例。
        /// </summary>
        public ShowItemFailureEventArgs()
        {
            ItemId = 0;
            ItemAssetName = null;
            ItemGroupName = null;
            ErrorMessage = null;
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
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 创建显示物品失败事件。
        /// </summary>
        /// <param name="ItemId">物品编号。</param>
        /// <param name="ItemAssetName">物品资源名称。</param>
        /// <param name="ItemGroupName">物品组名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的显示物品失败事件。</returns>
        public static ShowItemFailureEventArgs Create(int ItemId, string ItemAssetName, string ItemGroupName, string errorMessage, object userData)
        {
            ShowItemFailureEventArgs showItemFailureEventArgs = ReferencePool.Acquire<ShowItemFailureEventArgs>();
            showItemFailureEventArgs.ItemId = ItemId;
            showItemFailureEventArgs.ItemAssetName = ItemAssetName;
            showItemFailureEventArgs.ItemGroupName = ItemGroupName;
            showItemFailureEventArgs.ErrorMessage = errorMessage;
            showItemFailureEventArgs.UserData = userData;
            return showItemFailureEventArgs;
        }

        /// <summary>
        /// 清理显示物品失败事件。
        /// </summary>
        public override void Clear()
        {
            ItemId = 0;
            ItemAssetName = null;
            ItemGroupName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
