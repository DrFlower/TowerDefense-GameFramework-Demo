using GameFramework;
using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 显示物体更新事件。
    /// </summary>
    public sealed class ShowItemUpdateEventArgs : GameEventArgs
    {
        /// <summary>
        /// 显示物体更新事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ShowItemUpdateEventArgs).GetHashCode();

        /// <summary>
        /// 初始化显示物体更新事件的新实例。
        /// </summary>
        public ShowItemUpdateEventArgs()
        {
            ItemId = 0;
            ItemLogicType = null;
            ItemAssetName = null;
            ItemGroupName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取显示物体更新事件编号。
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
        /// 获取显示物体进度。
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
        /// 创建显示物体更新事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的显示物体更新事件。</returns>
        public static ShowItemUpdateEventArgs Create(GameFramework.Item.ShowItemUpdateEventArgs e)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)e.UserData;
            ShowItemUpdateEventArgs showItemUpdateEventArgs = ReferencePool.Acquire<ShowItemUpdateEventArgs>();
            showItemUpdateEventArgs.ItemId = e.ItemId;
            showItemUpdateEventArgs.ItemLogicType = showItemInfo.ItemLogicType;
            showItemUpdateEventArgs.ItemAssetName = e.ItemAssetName;
            showItemUpdateEventArgs.ItemGroupName = e.ItemGroupName;
            showItemUpdateEventArgs.Progress = e.Progress;
            showItemUpdateEventArgs.UserData = showItemInfo.UserData;
            return showItemUpdateEventArgs;
        }

        /// <summary>
        /// 清理显示物体更新事件。
        /// </summary>
        public override void Clear()
        {
            ItemId = 0;
            ItemLogicType = null;
            ItemAssetName = null;
            ItemGroupName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}
