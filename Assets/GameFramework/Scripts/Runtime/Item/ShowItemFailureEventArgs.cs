using GameFramework;
using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 显示物体失败事件。
    /// </summary>
    public sealed class ShowItemFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示物体失败事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ShowItemFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化显示物体失败事件的新实例。
        /// </summary>
        public ShowItemFailureEventArgs()
        {
            ItemId = 0;
            ItemLogicType = null;
            ItemAssetName = null;
            ItemGroupName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取显示物体失败事件编号。
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
        /// 创建显示物体失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的显示物体失败事件。</returns>
        public static ShowItemFailureEventArgs Create(GameFramework.Item.ShowItemFailureEventArgs e)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)e.UserData;
            ShowItemFailureEventArgs showItemFailureEventArgs = ReferencePool.Acquire<ShowItemFailureEventArgs>();
            showItemFailureEventArgs.ItemId = e.ItemId;
            showItemFailureEventArgs.ItemLogicType = showItemInfo.ItemLogicType;
            showItemFailureEventArgs.ItemAssetName = e.ItemAssetName;
            showItemFailureEventArgs.ItemGroupName = e.ItemGroupName;
            showItemFailureEventArgs.ErrorMessage = e.ErrorMessage;
            showItemFailureEventArgs.UserData = showItemInfo.UserData;
            ReferencePool.Release(showItemInfo);
            return showItemFailureEventArgs;
        }

        /// <summary>
        /// 清理显示物体失败事件。
        /// </summary>
        public override void Clear()
        {
            ItemId = 0;
            ItemLogicType = null;
            ItemAssetName = null;
            ItemGroupName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
