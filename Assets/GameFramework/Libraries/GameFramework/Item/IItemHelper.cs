namespace GameFramework.Item
{
    /// <summary>
    /// 物品辅助器接口。
    /// </summary>
    public interface IItemHelper
    {
        /// <summary>
        /// 实例化物品。
        /// </summary>
        /// <param name="entityAsset">要实例化的物品资源。</param>
        /// <returns>实例化后的物品。</returns>
        object InstantiateItem(object entityAsset);

        /// <summary>
        /// 创建物品。
        /// </summary>
        /// <param name="entityInstance">物品实例。</param>
        /// <param name="entityGroup">物品所属的物品组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>物品。</returns>
        IItem CreateItem(object itemInstance, IItemGroup itemGroup, object userData);

        /// <summary>
        /// 释放物品。
        /// </summary>
        /// <param name="entityAsset">要释放的物品资源。</param>
        /// <param name="entityInstance">要释放的物品实例。</param>
        void ReleaseItem(object itemAsset, object itemInstance);
    }

}
