using GameFramework.Item;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 物体辅助器基类。
    /// </summary>
    public abstract class ItemHelperBase : MonoBehaviour, IItemHelper
    {
        /// <summary>
        /// 实例化物体。
        /// </summary>
        /// <param name="itemAsset">要实例化的物体资源。</param>
        /// <returns>实例化后的物体。</returns>
        public abstract object InstantiateItem(object itemAsset);

        /// <summary>
        /// 创建物体。
        /// </summary>
        /// <param name="itemInstance">物体实例。</param>
        /// <param name="itemGroup">物体所属的物体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>物体。</returns>
        public abstract IItem CreateItem(object itemInstance, IItemGroup itemGroup, object userData);

        /// <summary>
        /// 释放物体。
        /// </summary>
        /// <param name="itemAsset">要释放的物体资源。</param>
        /// <param name="itemInstance">要释放的物体实例。</param>
        public abstract void ReleaseItem(object itemAsset, object itemInstance);
    }
}
