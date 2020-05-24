using GameFramework;
using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 显示物体成功事件。
    /// </summary>
    public sealed class ShowItemSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示物体成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ShowItemSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化显示物体成功事件的新实例。
        /// </summary>
        public ShowItemSuccessEventArgs()
        {
            ItemLogicType = null;
            Item = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取显示物体成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
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
        /// 获取显示成功的物体。
        /// </summary>
        public Item Item
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
        /// 创建显示物体成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的显示物体成功事件。</returns>
        public static ShowItemSuccessEventArgs Create(GameFramework.Item.ShowItemSuccessEventArgs e)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)e.UserData;
            ShowItemSuccessEventArgs showItemSuccessEventArgs = ReferencePool.Acquire<ShowItemSuccessEventArgs>();
            showItemSuccessEventArgs.ItemLogicType = showItemInfo.ItemLogicType;
            showItemSuccessEventArgs.Item = (Item)e.Item;
            showItemSuccessEventArgs.Duration = e.Duration;
            showItemSuccessEventArgs.UserData = showItemInfo.UserData;
            ReferencePool.Release(showItemInfo);
            return showItemSuccessEventArgs;
        }

        /// <summary>
        /// 清理显示物体成功事件。
        /// </summary>
        public override void Clear()
        {
            ItemLogicType = null;
            Item = null;
            Duration = 0f;
            UserData = null;
        }
    }
}
