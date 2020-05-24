using GameFramework;
using GameFramework.Item;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 隐藏物体完成事件。
    /// </summary>
    public sealed class HideItemCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 隐藏物体完成事件编号。
        /// </summary>
        public static readonly int EventId = typeof(HideItemCompleteEventArgs).GetHashCode();

        /// <summary>
        /// 初始化隐藏物体完成事件的新实例。
        /// </summary>
        public HideItemCompleteEventArgs()
        {
            ItemId = 0;
            ItemAssetName = null;
            ItemGroup = null;
            UserData = null;
        }

        /// <summary>
        /// 获取隐藏物体完成事件编号。
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
        /// 获取物体资源名称。
        /// </summary>
        public string ItemAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物体所属的物体组。
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
        /// 创建隐藏物体完成事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的隐藏物体完成事件。</returns>
        public static HideItemCompleteEventArgs Create(GameFramework.Item.HideItemCompleteEventArgs e)
        {
            HideItemCompleteEventArgs hideItemCompleteEventArgs = ReferencePool.Acquire<HideItemCompleteEventArgs>();
            hideItemCompleteEventArgs.ItemId = e.ItemId;
            hideItemCompleteEventArgs.ItemAssetName = e.ItemAssetName;
            hideItemCompleteEventArgs.ItemGroup = e.ItemGroup;
            hideItemCompleteEventArgs.UserData = e.UserData;
            return hideItemCompleteEventArgs;
        }

        /// <summary>
        /// 清理隐藏物体完成事件。
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
