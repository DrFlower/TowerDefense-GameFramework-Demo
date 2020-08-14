using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;
using GameFramework;

namespace Flower
{
    partial class LevelControl : IReference
    {
        private Level level;
        private LevelManager levelManager;
        private CameraInput cameraInput;

        private EntityLoader entityLoader;

        private int? uiMaskFormSerialId;

        private DataLevel dataLevel;
        private DataPlayer dataPlayer;
        private DataTower dataTower;
        private DataEnemy dataEnemy;

        private TowerData previewTowerData;
        private Entity previewTowerEntity;
        private EntityTowerPreview previewTowerEntityLogic;
        private bool isBuilding = false;
        private bool pause = false;

        private Dictionary<int, TowerInfo> dicTowerInfo;
        private Dictionary<int, EntityEnemy> dicEntityEnemy;

        public LevelControl()
        {
            dicTowerInfo = new Dictionary<int, TowerInfo>();
            dicEntityEnemy = new Dictionary<int, EntityEnemy>();
        }

        public void OnEnter()
        {
            entityLoader = EntityLoader.Create(this);
            dataLevel = GameEntry.Data.GetData<DataLevel>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataTower = GameEntry.Data.GetData<DataTower>();
            dataEnemy = GameEntry.Data.GetData<DataEnemy>();

            GameEntry.UI.OpenUIForm(EnumUIForm.UILevelMainInfoForm);
            GameEntry.UI.OpenUIForm(EnumUIForm.UITowerListForm);

            entityLoader.ShowEntity<EntityPlayer>(EnumEntity.Player, null, EntityData.Create(level.PlayerPosition, level.PlayerQuaternion));
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (level == null)
                return;

            if (dataLevel.LevelState != EnumLevelState.Prepare && dataLevel.LevelState != EnumLevelState.Normal)
                return;

            if (!level.Finish)
                level.ProcessLevel(elapseSeconds, realElapseSeconds);

            if (isBuilding)
            {
                if (Input.GetMouseButtonDown(0) && previewTowerEntityLogic != null && previewTowerEntityLogic.CanPlace)
                {
                    previewTowerEntityLogic.TryBuildTower();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    HidePreviewTower();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(ray, out raycastHit, float.MaxValue, LayerMask.GetMask("Towers")))
                    {
                        if (raycastHit.collider != null)
                        {
                            EntityTowerBase entityTowerBase = raycastHit.collider.gameObject.GetComponent<EntityTowerBase>();
                            if (entityTowerBase != null)
                            {
                                entityTowerBase.ShowControlForm();
                            }
                        }
                    }
                }
            }
        }

        public void ShowPreviewTower(TowerData towerData)
        {
            if (towerData == null)
                return;

            previewTowerData = towerData;
            uiMaskFormSerialId = GameEntry.UI.OpenUIForm(EnumUIForm.UIMask);


            entityLoader.ShowEntity<EntityTowerPreview>(towerData.PreviewEntityId, (entity) =>
             {
                 previewTowerEntity = entity;
                 previewTowerEntityLogic = entity.Logic as EntityTowerPreview;
                 if (previewTowerEntityLogic == null)
                 {
                     Log.Error("Entity '{0}' logic type invaild, need EntityTowerPreview", previewTowerEntity.Id);
                     return;
                 }

                 TowerLevelData towerLevelData = towerData.GetTowerLevelData(0);
                 if (towerLevelData == null)
                 {
                     Log.Error("Tower '{0}' Level '{1}' data is null.", towerData.Name, 0);
                 }

                 EntityDataRadiusVisualiser entityDataRadiusVisualiser = EntityDataRadiusVisualiser.Create(towerLevelData.Range);

                 entityLoader.ShowEntity<EntityRadiusVisualizer>(EnumEntity.RadiusVisualiser, (entityRadiusVisualizer) =>
                 {
                     GameEntry.Entity.AttachEntity(entityRadiusVisualizer, previewTowerEntity);
                 },
                 entityDataRadiusVisualiser);

                 isBuilding = true;
             },
             EntityDataTowerPreview.Create(towerData));
        }

        public void HidePreviewTower()
        {
            if (uiMaskFormSerialId != null)
                GameEntry.UI.CloseUIForm((int)uiMaskFormSerialId);

            GameEntry.Event.Fire(this, HidePreviewTowerEventArgs.Create(previewTowerData));

            if (previewTowerEntity != null)
                entityLoader.HideEntity(previewTowerEntity);

            uiMaskFormSerialId = null;

            previewTowerEntity = null;
            previewTowerData = null;

            isBuilding = false;
        }

        public void CreateTower(TowerData towerData, IPlacementArea placementArea, IntVector2 placeGrid, Vector3 position, Quaternion rotation)
        {
            if (towerData == null)
                return;

            TowerLevelData towerLevelData = towerData.GetTowerLevelData(0);

            if (dataPlayer.Energy < towerLevelData.BuildEnergy)
                return;

            dataPlayer.AddEnergy(-towerLevelData.BuildEnergy);

            Tower tower = dataTower.CreateTower(towerData.Id);

            if (tower == null)
            {
                Log.Error("Create tower fail,Tower data id is '{0}'.", towerData.Id);
                return;
            }

            entityLoader.ShowEntity(towerData.EntityId, TypeUtility.GetEntityType(tower.Type),
            (entity) =>
            {
                EntityTowerBase entityTowerBase = entity.Logic as EntityTowerBase;
                dicTowerInfo.Add(tower.SerialId, TowerInfo.Create(tower, entityTowerBase, placementArea, placeGrid));
            }
            , EntityDataTower.Create(tower, position, rotation));

            HidePreviewTower();
        }

        public void HideTower(int towerSerialId)
        {
            if (!dicTowerInfo.ContainsKey(towerSerialId))
                return;

            TowerInfo towerInfo = dicTowerInfo[towerSerialId];
            entityLoader.HideEntity(dicTowerInfo[towerSerialId].EntityTower.Entity);
            towerInfo.PlacementArea.Clear(towerInfo.PlaceGrid, towerInfo.Tower.Dimensions);
            dataTower.DestroyTower(towerInfo.Tower);
            dicTowerInfo.Remove(towerSerialId);
            ReferencePool.Release(towerInfo);
        }

        private void HideAllTower()
        {
            List<int> towerSerialIds = new List<int>(dicTowerInfo.Keys);
            for (int i = 0; i < towerSerialIds.Count; i++)
            {
                HideTower(towerSerialIds[i]);
            }
        }

        public void SpawnEnemy(int enemyId)
        {
            EnemyData enemyData = dataEnemy.GetEnemyData(enemyId);

            if (enemyData == null)
            {
                Log.Error("Can not get enemy data by id '{0}'.", enemyId);
                return;
            }

            entityLoader.ShowEntity(enemyData.EntityId, TypeUtility.GetEntityType(enemyData.Type),
                (entity) =>
                {
                    dicEntityEnemy.Add(entity.Id, (EntityEnemy)entity.Logic);
                },
                EntityDataEnemy.Create(
                    enemyData,
                    levelManager.GetLevelPath(),
                    levelManager.GetStartPathNode().position - new Vector3(0, 0.2f, 0),
                    Quaternion.identity));
        }

        public void HideEnemyEntity(int serialId)
        {
            if (!dicEntityEnemy.ContainsKey(serialId))
            {
                Log.Error("Can't find enemy entity('serial id:{0}') ", serialId);
                return;
            }

            entityLoader.HideEntity(serialId);
            dicEntityEnemy.Remove(serialId);

            if (level.Finish && dicEntityEnemy.Count <= 0)
                dataLevel.GameSuccess();
        }

        private void HideAllEnemyEntity()
        {
            foreach (var item in dicEntityEnemy.Values)
            {
                entityLoader.HideEntity(item.Entity);
            }

            dicEntityEnemy.Clear();
        }

        public void ShowEntity(int entityId, Type entityType, Action<Entity> showSuccess, EntityData entityData)
        {
            entityLoader.ShowEntity(entityId, entityType, (entity) =>
            {
                //这里处理一下，如果暂停前调用ShowEntity，暂停后才成功加载出来调用Entity的OnShow的话，那这个Entity是没被执行到IPause的逻辑的，这里在ShowEntity成功的回调下补充个调用IPause的逻辑
                if (pause == true)
                {
                    IPause iPause = entity.Logic as IPause;
                    if (iPause != null)
                    {
                        iPause.Pause();
                    }
                }
                if (showSuccess != null)
                    showSuccess(entity);
            }, entityData);
        }

        public void HideEntity(int entityId)
        {
            entityLoader.HideEntity(entityId);
        }

        public void StartWave()
        {
            level.StartWave();
        }

        public void Pause()
        {
            pause = true;

            foreach (var entity in entityLoader.GetAllEntities())
            {
                IPause iPause = entity.Logic as IPause;
                if (iPause != null)
                    iPause.Pause();
            }

            cameraInput.Pause();
        }

        public void Resume()
        {
            pause = false;

            foreach (var entity in entityLoader.GetAllEntities())
            {
                IPause iPause = entity.Logic as IPause;
                if (iPause != null)
                    iPause.Resume();
            }

            cameraInput.Resume();
        }

        public void Restart()
        {
            if (pause)
            {
                Resume();
                pause = false;
            }

            HideAllTower();
            HideAllEnemyEntity();
        }

        public void Gameover(EnumGameOverType enumGameOverType, int starCount)
        {
            HidePreviewTower();
            Pause();
            GameEntry.UI.OpenUIForm(EnumUIForm.UIGameOverForm, UIGameOverFormOpenParam.Create(level.LevelData, enumGameOverType, starCount));
        }

        public void Quick()
        {
            if (pause)
            {
                Resume();
                pause = false;
            }

            HideAllTower();
            entityLoader.HideAllEntity();
        }

        public static LevelControl Create(Level level, LevelManager levelPathManager, CameraInput cameraInput)
        {
            LevelControl levelControl = ReferencePool.Acquire<LevelControl>();
            levelControl.level = level;
            levelControl.levelManager = levelPathManager;
            levelControl.cameraInput = cameraInput;
            return levelControl;
        }

        public void Clear()
        {
            level = null;
            levelManager = null;
            cameraInput = null;

            if (entityLoader != null)
                ReferencePool.Release(entityLoader);

            entityLoader = null;

            uiMaskFormSerialId = null;

            dataPlayer = null;
            dataTower = null;

            previewTowerData = null;
            previewTowerEntity = null;
            previewTowerEntityLogic = null;
            isBuilding = false;

            dicTowerInfo.Clear();
            dicEntityEnemy.Clear();
        }
    }
}
