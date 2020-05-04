using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Procedure;
using GameFramework.DataTable;
using UnityEngine;

using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using LoadDataTableSuccessEventArgs = UnityGameFramework.Runtime.LoadDataTableSuccessEventArgs;
using LoadDataTableFailureEventArgs = UnityGameFramework.Runtime.LoadDataTableFailureEventArgs;

namespace Flower
{
    public class ProcedurePreload : ProcedureBase
    {
        public static readonly string[] dataTableNames = new string[]
        {
            "Scene",
            "UIForm",
            "Sound",
            "SoundGroup",
            "SoundAssets",
            "SoundPlayParam",
        };

        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            PreloadResources();            
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            foreach (var item in m_LoadedFlag)
            {
                if (!item.Value)
                    return;
            }

            SetComponents();
            procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Menu"));
            ChangeState<ProcedureLoadingScene>(procedureOwner);

        }


        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void PreloadResources()
        {
            // Preload configs
            LoadConfig("DefaultConfig");

            // Preload data tables
            foreach (string dataTableName in dataTableNames)
            {
                LoadDataTable(dataTableName);
            }

            // Preload dictionaries
            LoadDictionary("Default");
        }

        private void SetComponents()
        {
            SetSoundComponent();

        }

        private void SetSoundComponent()
        {
            IDataTable<DRSoundGroup> dtSoundGroup = GameEntry.DataTable.GetDataTable<DRSoundGroup>();
            DRSoundGroup[] dRSoundGroups = dtSoundGroup.GetAllDataRows();
            foreach (var item in dRSoundGroups)
            {
                GameEntry.Sound.AddSoundGroup(item.Name, item.AvoidBeingReplacedBySamePriority, item.Mute, item.Volume, item.SoundAgentCount);
            }
        }

        private void LoadConfig(string configName)
        {
            m_LoadedFlag.Add(Utility.Text.Format("Config.{0}", configName), false);
            GameEntry.Config.LoadConfig(configName, false, this);
        }


        private void LoadDataTable(string dataTableName)
        {
            m_LoadedFlag.Add(Utility.Text.Format("DataTable.{0}", dataTableName), false);
            GameEntry.DataTable.LoadDataTable(dataTableName, true, this);
        }

        private void LoadDictionary(string dictionaryName)
        {
            m_LoadedFlag.Add(Utility.Text.Format("Dictionary.{0}", dictionaryName), false);
            GameEntry.Localization.LoadDictionary(dictionaryName, false, this);
        }

        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[Utility.Text.Format("Config.{0}", ne.ConfigName)] = true;
            Log.Info("Load config '{0}' OK.", ne.ConfigName);
        }

        private void OnLoadConfigFailure(object sender, GameEventArgs e)
        {
            LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigName, ne.ConfigAssetName, ne.ErrorMessage);
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[Utility.Text.Format("DataTable.{0}", ne.DataTableName)] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableName);
        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);
        }

        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[Utility.Text.Format("Dictionary.{0}", ne.DictionaryName)] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryName);
        }

        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryName, ne.DictionaryAssetName, ne.ErrorMessage);
        }

    }
}

