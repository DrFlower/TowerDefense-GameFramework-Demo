using GameFramework.Item;
using UnityEngine;
namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认物体组辅助器。
    /// </summary>
    public class DefaultItemHelper : ItemHelperBase
    {
        private ResourceComponent m_ResourceComponent = null;

        /// <summary>
        /// 实例化实体。
        /// </summary>
        /// <param name="itemAsset">要实例化的实体资源。</param>
        /// <returns>实例化后的实体。</returns>
        public override object InstantiateItem(object itemAsset)
        {
            return Instantiate((Object)itemAsset);
        }

        /// <summary>
        /// 创建实体。
        /// </summary>
        /// <param name="itemInstance">实体实例。</param>
        /// <param name="itemGroup">实体所属的实体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>实体。</returns>
        public override IItem CreateItem(object itemInstance, IItemGroup itemGroup, object userData)
        {
            GameObject gameObject = itemInstance as GameObject;
            if (gameObject == null)
            {
                Log.Error("Item instance is invalid.");
                return null;
            }

            Transform transform = gameObject.transform;
            transform.SetParent(((MonoBehaviour)itemGroup.Helper).transform);
            return gameObject.GetOrAddComponent<Item>();
        }

        /// <summary>
        /// 释放实体。
        /// </summary>
        /// <param name="itemAsset">要释放的实体资源。</param>
        /// <param name="itemInstance">要释放的实体实例。</param>
        public override void ReleaseItem(object itemAsset, object itemInstance)
        {
            m_ResourceComponent.UnloadAsset(itemAsset);
            Destroy((Object)itemInstance);
        }

        private void Start()
        {
            m_ResourceComponent = GameEntry.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }
        }
    }
}
