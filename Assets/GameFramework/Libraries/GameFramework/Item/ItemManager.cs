using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Item
{
    internal sealed partial class ItemManager : GameFrameworkModule, IItemManager
    {
        private readonly Dictionary<int, ItemInfo> m_ItemInfos;
        private readonly Dictionary<string, IItemGroup> m_ItemGroups;
        private readonly Dictionary<int, int> m_EntitiesBeingLoaded;
        private readonly HashSet<int> m_EntitiesToReleaseOnLoad;
        private readonly Queue<ItemInfo> m_RecycleQueue;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IObjectPoolManager m_ObjectPoolManager;
        private IResourceManager m_ResourceManager;
        private IItemManager m_ItemHelper;
        private int m_Serial;
        private bool m_IsShutdown;
        private EventHandler<ShowItemSuccessEventArgs> m_ShowItemSuccessEventHandler;
        private EventHandler<ShowItemFailureEventArgs> m_ShowItemFailureEventHandler;
        private EventHandler<ShowItemUpdateEventArgs> m_ShowItemUpdateEventHandler;
        private EventHandler<ShowItemDependencyAssetEventArgs> m_ShowItemDependencyAssetEventHandler;
        private EventHandler<HideItemCompleteEventArgs> m_HideItemCompleteEventHandler;

        public ItemManager()
        {
            m_ItemInfos = new Dictionary<int, ItemInfo>();
            m_ItemGroups = new Dictionary<string, IItemGroup>();
            m_EntitiesBeingLoaded = new Dictionary<int, int>();
            m_EntitiesToReleaseOnLoad = new HashSet<int>();
            m_RecycleQueue = new Queue<ItemInfo>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback, LoadAssetUpdateCallback, LoadAssetDependencyAssetCallback);
            m_ObjectPoolManager = null;
            m_ResourceManager = null;
            m_ItemHelper = null;
            m_Serial = 0;
            m_IsShutdown = false;
            m_ShowItemSuccessEventHandler = null;
            m_ShowItemFailureEventHandler = null;
            m_ShowItemUpdateEventHandler = null;
            m_ShowItemDependencyAssetEventHandler = null;
            m_HideItemCompleteEventHandler = null;
        }

        internal override void Shutdown()
        {

        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }




        private void LoadAssetSuccessCallback(string ItemAssetName, object ItemAsset, float duration, object userData)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)userData;
            if (showItemInfo == null)
            {
                throw new GameFrameworkException("Show Item info is invalid.");
            }

            if (m_EntitiesToReleaseOnLoad.Contains(showItemInfo.SerialId))
            {
                m_EntitiesToReleaseOnLoad.Remove(showItemInfo.SerialId);
                ReferencePool.Release(showItemInfo);
                m_ItemHelper.ReleaseItem(ItemAsset, null);
                return;
            }

            m_EntitiesBeingLoaded.Remove(showItemInfo.ItemId);
            ItemInstanceObject ItemInstanceObject = ItemInstanceObject.Create(ItemAssetName, ItemAsset, m_ItemHelper.InstantiateItem(ItemAsset), m_ItemHelper);
            showItemInfo.ItemGroup.RegisterItemInstanceObject(ItemInstanceObject, true);

            InternalShowItem(showItemInfo.ItemId, ItemAssetName, showItemInfo.ItemGroup, ItemInstanceObject.Target, true, duration, showItemInfo.UserData);
            ReferencePool.Release(showItemInfo);
        }

        private void LoadAssetFailureCallback(string ItemAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)userData;
            if (showItemInfo == null)
            {
                throw new GameFrameworkException("Show Item info is invalid.");
            }

            if (m_EntitiesToReleaseOnLoad.Contains(showItemInfo.SerialId))
            {
                m_EntitiesToReleaseOnLoad.Remove(showItemInfo.SerialId);
                return;
            }

            m_EntitiesBeingLoaded.Remove(showItemInfo.ItemId);
            string appendErrorMessage = Utility.Text.Format("Load Item failure, asset name '{0}', status '{1}', error message '{2}'.", ItemAssetName, status.ToString(), errorMessage);
            if (m_ShowItemFailureEventHandler != null)
            {
                ShowItemFailureEventArgs showItemFailureEventArgs = ShowItemFailureEventArgs.Create(showItemInfo.ItemId, ItemAssetName, showItemInfo.ItemGroup.Name, appendErrorMessage, showItemInfo.UserData);
                m_ShowItemFailureEventHandler(this, showItemFailureEventArgs);
                ReferencePool.Release(showItemFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string ItemAssetName, float progress, object userData)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)userData;
            if (showItemInfo == null)
            {
                throw new GameFrameworkException("Show Item info is invalid.");
            }

            if (m_ShowItemUpdateEventHandler != null)
            {
                ShowItemUpdateEventArgs showItemUpdateEventArgs = ShowItemUpdateEventArgs.Create(showItemInfo.ItemId, ItemAssetName, showItemInfo.ItemGroup.Name, progress, showItemInfo.UserData);
                m_ShowItemUpdateEventHandler(this, showItemUpdateEventArgs);
                ReferencePool.Release(showItemUpdateEventArgs);
            }
        }

        private void LoadAssetDependencyAssetCallback(string ItemAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            ShowItemInfo showItemInfo = (ShowItemInfo)userData;
            if (showItemInfo == null)
            {
                throw new GameFrameworkException("Show Item info is invalid.");
            }

            if (m_ShowItemDependencyAssetEventHandler != null)
            {
                ShowItemDependencyAssetEventArgs showItemDependencyAssetEventArgs = ShowItemDependencyAssetEventArgs.Create(showItemInfo.ItemId, ItemAssetName, showItemInfo.ItemGroup.Name, dependencyAssetName, loadedCount, totalCount, showItemInfo.UserData);
                m_ShowItemDependencyAssetEventHandler(this, showItemDependencyAssetEventArgs);
                ReferencePool.Release(showItemDependencyAssetEventArgs);
            }
        }
    }
}

