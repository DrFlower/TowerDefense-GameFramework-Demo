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
            //m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback, LoadAssetUpdateCallback, LoadAssetDependencyAssetCallback);
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
    }
}

