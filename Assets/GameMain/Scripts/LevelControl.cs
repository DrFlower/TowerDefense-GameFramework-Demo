using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;
using GameFramework;

namespace Flower
{
    class LevelControl
    {
        private EntityLoader entityLoader;

        private int? uiMaskFormSerialId;

        private TowerData currentShowPreviewTower;
        private Entity currentShowTowerEntity;

        public void Enter()
        {
            entityLoader = EntityLoader.Create(this);

            GameEntry.UI.OpenUIForm(EnumUIForm.UILevelMainInfoForm);
            GameEntry.UI.OpenUIForm(EnumUIForm.UITowerListForm);
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (Input.GetMouseButtonDown(1))
            {
                HidePreviewTower();
            }
        }

        public void ShowPreviewTower(TowerData towerData)
        {
            if (towerData == null)
                return;

            currentShowPreviewTower = towerData;
            uiMaskFormSerialId = GameEntry.UI.OpenUIForm(EnumUIForm.UIMask);


            entityLoader.ShowEntity<EntityTowerPreview>(towerData.EntityId, (entity) =>
             {
                 currentShowTowerEntity = entity;

                 TowerLevelData towerLevelData = towerData.GetTowerLevelData(0);
                 if (towerLevelData == null)
                 {
                     Log.Error("Tower '{0}' Level '{1}' data is null.", towerData.Name, 0);
                 }

                 EntityDataRadiusVisualiser entityDataRadiusVisualiser = EntityDataRadiusVisualiser.Create(towerLevelData.Range);

                 entityLoader.ShowEntity<EntityRadiusVisualizer>(EnumEntity.RadiusVisualiser, (entityRadiusVisualizer) =>
                 {
                     GameEntry.Entity.AttachEntity(entityRadiusVisualizer, currentShowTowerEntity);
                 },
                 entityDataRadiusVisualiser);
             },
             EntityData.Create());
        }

        public void HidePreviewTower()
        {
            if (uiMaskFormSerialId != null)
                GameEntry.UI.CloseUIForm((int)uiMaskFormSerialId);

            GameEntry.Event.Fire(this, HidePreviewTowerEventArgs.Create(currentShowPreviewTower));

            if (currentShowTowerEntity != null)
                entityLoader.HideEntity(currentShowTowerEntity);

            currentShowTowerEntity = null;
            currentShowPreviewTower = null;
        }

        public void CreateTower(Tower tower)
        {

        }

        public void StartWave()
        {

        }

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void Gameover()
        {

        }

        public void Quick()
        {

        }

    }
}
