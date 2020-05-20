namespace GameFramework.Item
{
    /// <summary>
    /// 显示物品成功事件。
    /// </summary>
    public sealed class ShowItemSuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化显示物品成功事件的新实例。
        /// </summary>
        public ShowItemSuccessEventArgs()
        {
            Item = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取显示成功的物品。
        /// </summary>
        public IItem Item
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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
        /// 创建显示物品成功事件。
        /// </summary>
        /// <param name="Item">加载成功的物品。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的显示物品成功事件。</returns>
        public static ShowItemSuccessEventArgs Create(IItem Item, float duration, object userData)
        {
            ShowItemSuccessEventArgs showItemSuccessEventArgs = ReferencePool.Acquire<ShowItemSuccessEventArgs>();
            showItemSuccessEventArgs.Item = Item;
            showItemSuccessEventArgs.Duration = duration;
            showItemSuccessEventArgs.UserData = userData;
            return showItemSuccessEventArgs;
        }

        /// <summary>
        /// 清理显示物品成功事件。
        /// </summary>
        public override void Clear()
        {
            Item = null;
            Duration = 0f;
            UserData = null;
        }
    }
}

