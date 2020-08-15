using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;
using System.Collections.Generic;

namespace Flower.Data
{
    public abstract class DataBase : GameFramework.Data.Data
    {
        private Dictionary<string, bool> loadedFlag = new Dictionary<string, bool>();
        private EventSubscriber eventSubscriber;

        public bool IsPreloadReady
        {
            get
            {
                foreach (var item in loadedFlag)
                {
                    if (!item.Value)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public sealed override void Init()
        {
            OnInit();
        }

        public sealed override void Preload()
        {
            GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            OnPreload();
        }

        public sealed override void Load()
        {
            GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            OnLoad();
        }

        public sealed override void Unload()
        {
            if (eventSubscriber != null)
            {
                eventSubscriber.UnSubscribeAll();
                ReferencePool.Release(eventSubscriber);
                eventSubscriber = null;
            }

            OnUnload();
        }

        public sealed override void Shutdown()
        {
            OnShutdown();
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnPreload()
        {
        }

        protected virtual void OnLoad()
        {
        }

        protected virtual void OnUnload()
        {
        }

        protected virtual void OnShutdown()
        {
        }

        protected void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (eventSubscriber == null)
                eventSubscriber = EventSubscriber.Create(this);

            eventSubscriber.Subscribe(id, handler);
        }

        protected void UnSubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (eventSubscriber != null)
                eventSubscriber.UnSubscribe(id, handler);
        }

        protected void LoadConfig(string configName)
        {
            string configAssetName = AssetUtility.GetConfigAsset(configName, false);
            loadedFlag.Add(configAssetName, false);
            GameEntry.Config.ReadData(configAssetName, this);
        }

        protected void LoadDataTable(string dataTableName)
        {
            string dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName, true);
            loadedFlag.Add(dataTableAssetName, false);
            GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
        }

        protected void LoadDictionary(string dictionaryName)
        {
            string dictionaryAssetName = AssetUtility.GetDictionaryAsset(dictionaryName, false);
            loadedFlag.Add(dictionaryAssetName, false);
            GameEntry.Localization.ReadData(dictionaryAssetName, this);
        }

        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            loadedFlag[ne.ConfigAssetName] = true;
            Log.Info("Load config '{0}' OK.", ne.ConfigAssetName);
        }

        private void OnLoadConfigFailure(object sender, GameEventArgs e)
        {
            LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigAssetName, ne.ConfigAssetName, ne.ErrorMessage);
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            loadedFlag[ne.DataTableAssetName] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableAssetName);
        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableAssetName, ne.DataTableAssetName, ne.ErrorMessage);
        }

        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            loadedFlag[ne.DictionaryAssetName] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryAssetName);
        }

        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryAssetName, ne.DictionaryAssetName, ne.ErrorMessage);
        }
    }

}


