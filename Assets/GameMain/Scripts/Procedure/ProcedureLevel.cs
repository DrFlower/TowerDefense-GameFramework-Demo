using System.Collections;
using System.Collections.Generic;
using GameFramework.Localization;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Procedure;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Flower
{
    public class ProcedureLevel : ProcedureBase
    {
        private ProcedureOwner procedureOwner;
        private bool changeScene = false;

        private LevelControl levelControl;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            levelControl = new LevelControl();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(LoadLevelEventArgs.EventId, OnLoadLevel);
            //GameEntry.Event.Subscribe(ReloadLevelEventArgs.EventId, OnReloadLevel);

            this.procedureOwner = procedureOwner;
            this.changeScene = false;

            levelControl.Enter();       
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (changeScene)
            {
                ChangeState<ProcedureLoadingScene>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Unsubscribe(LoadLevelEventArgs.EventId, OnLoadLevel);
        
            //GameEntry.Event.Unsubscribe(ReloadLevelEventArgs.EventId, OnReloadLevel);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void OnStartWave(object sender, GameEventArgs e)
        {
            StartWaveEventArgs ne = (StartWaveEventArgs)e;
            if (ne == null)
                return;

            levelControl.StartWave();
        }

        private void OnGameover(object sender, GameEventArgs e)
        {
            GameoverEventArgs ne = (GameoverEventArgs)e;
            if (ne == null)
                return;

            levelControl.Gameover();
        }

        private void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            changeScene = true;
            procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, ne.SceneId);
        }

        private void OnLoadLevel(object sender, GameEventArgs e)
        {
            LoadLevelEventArgs ne = (LoadLevelEventArgs)e;
            if (ne == null)
                return;

            if (ne.LevelData == null)
            {
                Log.Error("Load level event param LevelData is null");
                return;
            }

            if (ne.LevelData.SceneData == null)
            {
                Log.Error("Load level event param SceneData is null");
                return;
            }

            changeScene = true;
            procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, ne.LevelData.SceneData.Id);
        }

        private void OnReloadLevel(object sender, GameEventArgs e)
        {
            ReloadLevelEventArgs ne = (ReloadLevelEventArgs)e;
            if (ne == null)
                return;

            if (ne.LevelData == null)
            {
                Log.Error("Load level event param LevelData is null");
                return;
            }

            if (ne.LevelData.SceneData == null)
            {
                Log.Error("Load level event param SceneData is null");
                return;
            }

            changeScene = true;
            procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, ne.LevelData.SceneData.Id);
        }

    }
}

