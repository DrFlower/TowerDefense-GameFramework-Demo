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

            LevelManager levelPathManager = UnityEngine.GameObject.Find("LevelManager").GetComponent<LevelManager>();
            if (levelPathManager == null)
            {
                Log.Error("Can not find LevelPathManager instance in scene");
                return;
            }

            CameraInput cameraInput = UnityEngine.GameObject.Find("GameCamera").GetComponent<CameraInput>();
            if (cameraInput == null)
            {
                Log.Error("Can not find CameraInput instance in scene");
                return;
            }

            levelControl = LevelControl.Create(dataLevel.CurrentLevel, levelPathManager, cameraInput);

            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(LoadLevelEventArgs.EventId, OnLoadLevel);
            GameEntry.Event.Subscribe(LevelStateChangeEventArgs.EventId, OnLevelStateChange);
            GameEntry.Event.Subscribe(ReloadLevelEventArgs.EventId, OnReloadLevel);
            GameEntry.Event.Subscribe(GameoverEventArgs.EventId, OnGameOver);
            GameEntry.Event.Subscribe(ShowPreviewTowerEventArgs.EventId, OnShowPreviewTower);
            GameEntry.Event.Subscribe(BuildTowerEventArgs.EventId, OnBuildTower);
            GameEntry.Event.Subscribe(HideTowerInLevelEventArgs.EventId, OnSellTower);
            GameEntry.Event.Subscribe(StartWaveEventArgs.EventId, OnStartWave);
            GameEntry.Event.Subscribe(SpawnEnemyEventArgs.EventId, OnSpawnEnemy);
            GameEntry.Event.Subscribe(HideEnemyEventArgs.EventId, OnHideEnemyEntity);
            GameEntry.Event.Subscribe(ShowEntityInLevelEventArgs.EventId, OnShowEntityInLevel);
            GameEntry.Event.Subscribe(HideEntityInLevelEventArgs.EventId, OnHideEntityInLevel);

            this.procedureOwner = procedureOwner;
            this.changeScene = false;

            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);

            GameEntry.UI.OpenDownloadForm();

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
            GameEntry.Event.Unsubscribe(LevelStateChangeEventArgs.EventId, OnLevelStateChange);
            GameEntry.Event.Unsubscribe(ReloadLevelEventArgs.EventId, OnReloadLevel);
            GameEntry.Event.Unsubscribe(GameoverEventArgs.EventId, OnGameOver);
            GameEntry.Event.Unsubscribe(ShowPreviewTowerEventArgs.EventId, OnShowPreviewTower);
            GameEntry.Event.Unsubscribe(BuildTowerEventArgs.EventId, OnBuildTower);
            GameEntry.Event.Unsubscribe(HideTowerInLevelEventArgs.EventId, OnSellTower);
            GameEntry.Event.Unsubscribe(StartWaveEventArgs.EventId, OnStartWave);
            GameEntry.Event.Unsubscribe(SpawnEnemyEventArgs.EventId, OnSpawnEnemy);
            GameEntry.Event.Unsubscribe(HideEnemyEventArgs.EventId, OnHideEnemyEntity);
            GameEntry.Event.Unsubscribe(ShowEntityInLevelEventArgs.EventId, OnShowEntityInLevel);
            GameEntry.Event.Unsubscribe(HideEntityInLevelEventArgs.EventId, OnHideEntityInLevel);

            levelControl.Quick();

            GameEntry.Sound.StopMusic();

            ReferencePool.Release(levelControl);
            levelControl = null;
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

        private void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            changeScene = true;
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
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
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.LevelData.SceneData.Id);
        }

        private void OnLevelStateChange(object sender, GameEventArgs e)
        {
            LevelStateChangeEventArgs ne = (LevelStateChangeEventArgs)e;
            if (ne == null)
                return;

            if (ne.CurrentState == EnumLevelState.Pause)
            {
                levelControl.Pause();
            }
            else if (ne.LastState == EnumLevelState.Pause)
            {
                levelControl.Resume();
            }
        }

        private void OnGameOver(object sender, GameEventArgs e)
        {
            GameoverEventArgs ne = (GameoverEventArgs)e;
            if (ne == null)
                return;

            levelControl.Gameover(ne.EnumGameOverType, ne.StarCount);
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
            HideTowerInLevelEventArgs ne = (HideTowerInLevelEventArgs)e;
            if (ne == null)
                return;

            if (levelControl == null)
                return;

            levelControl.HideTower(ne.TowerSerialId);
        }

        private void OnSpawnEnemy(object sender, GameEventArgs e)
        {
            SpawnEnemyEventArgs ne = (SpawnEnemyEventArgs)e;
            if (ne == null)
                return;

            levelControl.SpawnEnemy(ne.EnemyId);
        }

        private void OnHideEnemyEntity(object sender, GameEventArgs e)
        {
            HideEnemyEventArgs ne = (HideEnemyEventArgs)e;
            if (ne == null)
                return;

            levelControl.HideEnemyEntity(ne.EntityId);
        }

        private void OnShowEntityInLevel(object sender, GameEventArgs e)
        {
            ShowEntityInLevelEventArgs ne = (ShowEntityInLevelEventArgs)e;
            if (ne == null)
                return;

            levelControl.ShowEntity(ne.EntityId, ne.Type, ne.ShowSuccess, ne.EntityData);
        }

        private void OnHideEntityInLevel(object sender, GameEventArgs e)
        {
            HideEntityInLevelEventArgs ne = (HideEntityInLevelEventArgs)e;
            if (ne == null)
                return;

            levelControl.HideEntity(ne.EntityId);
        }

    }
}

