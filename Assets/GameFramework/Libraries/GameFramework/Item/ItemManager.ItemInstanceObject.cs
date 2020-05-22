using GameFramework.ObjectPool;

namespace GameFramework.Item
{
    internal sealed partial class ItemManager : GameFrameworkModule, IItemManager
    {
        private sealed class ItemInstanceObject : ObjectBase
        {
            private object m_ItemAsset;
            private IItemHelper m_ItemHelper;

            public ItemInstanceObject()
            {
                m_ItemAsset = null;
                m_ItemHelper = null;
            }

            public static ItemInstanceObject Create(string name, object itemAsset, object itemInstance, IItemHelper itemHelper)
            {
                if (itemAsset == null)
                {
                    throw new GameFrameworkException("Item asset is invalid.");
                }

                if (itemHelper == null)
                {
                    throw new GameFrameworkException("Item helper is invalid.");
                }

                ItemInstanceObject itemInstanceObject = ReferencePool.Acquire<ItemInstanceObject>();
                itemInstanceObject.Initialize(name, itemInstance);
                itemInstanceObject.m_ItemAsset = itemAsset;
                itemInstanceObject.m_ItemHelper = itemHelper;
                return itemInstanceObject;
            }

            public override void Clear()
            {
                base.Clear();
                m_ItemAsset = null;
                m_ItemHelper = null;
            }

            protected internal override void Release(bool isShutdown)
            {
                m_ItemHelper.ReleaseItem(m_ItemAsset, Target);
            }
        }
    }
}
