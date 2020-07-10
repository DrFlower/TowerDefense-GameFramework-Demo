using System.Collections;
using System.Collections.Generic;
using GameFramework.Localization;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Procedure;
using GameFramework;
using Flower.Data;
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
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();
            LevelData levelData = dataLevel.GetLevelData(dataLevel.CurrentLevel);

            LevelPathManager levelPathManager = UnityEngine.GameObject.Find("LevelPathManager").GetComponent<LevelPathManager>();
            if (levelPathManager == null)
            {
                Log.Error("Can not find LevelPathManager instance in scene");
                return;
            }

            levelControl = LevelControl.Create(levelData, levelPathManager);

            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(LoadLevelEventArgs.EventId, OnLoadLevel);
            GameEntry.Event.Subscribe(ReloadLevelEventArgs.EventId, OnReloadLevel);
            GameEntry.Event.Subscribe(ShowPreviewTowerEventArgs.EventId, OnShowPreviewTower);
            GameEntry.Event.Subscribe(BuildTowerEventArgs.EventId, OnBuildTower);
            GameEntry.Event.Subscribe(SellTowerEventArgs.EventId, OnSellTower);
            GameEntry.Event.Subscribe(StartWaveEventArgs.EventId, OnStartWave);

            this.procedureOwner = procedureOwner;
            this.changeScene = false;

            levelControl.OnEnter();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (changeScene)
            {
                ChangeState<ProcedureLoadingScene>(procedureOwner);
            }

            if (levelControl != null)
                levelControl.Update(elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Unsubscribe(LoadLevelEventArgs.EventId, OnLoadLevel);
            GameEntry.Event.Unsubscribe(ReloadLevelEventArgs.EventId, OnReloadLevel);
            GameEntry.Event.Unsubscribe(ShowPreviewTowerEventArgs.EventId, OnShowPreviewTower);
            GameEntry.Event.Unsubscribe(BuildTowerEventArgs.EventId, OnBuildTower);
            GameEntry.Event.Unsubscribe(SellTowerEventArgs.EventId, OnSellTower);
            GameEntry.Event.Unsubscribe(StartWaveEventArgs.EventId, OnStartWave);

            levelControl.Quick();
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            ReferencePool.Release(levelControl);
            levelControl = null;
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

            levelControl.Restart();
        }

        private void OnShowPreviewTower(object sender, GameEventArgs e)
        {
            ShowPreviewTowerEventArgs ne = (ShowPreviewTowerEventArgs)e;
            if (ne == null)
                return;

            if (levelControl == null)
                return;

            levelControl.ShowPreviewTower(ne.TowerData);
        }

        private void OnBuildTower(object sender, GameEventArgs e)
        {
            BuildTowerEventArgs ne = (BuildTowerEventArgs)e;
            if (ne == null)
                return;

            if (levelControl == null)
                return;

            levelControl.CreateTower(ne.TowerData, ne.PlacementArea, ne.PlaceGrid, ne.Position, ne.Rotation);
        }

        private void OnSellTower(object sender, GameEventArgs e)
        {
            SellTowerEventArgs ne = (SellTowerEventArgs)e;
            if (ne == null)
                return;

            if (levelControl == null)
                return;

            levelControl.DestroyTower(ne.TowerSerialId);
        }
    }
}

