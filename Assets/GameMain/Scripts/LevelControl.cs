using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;

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

            if (currentShowTowerEntity != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    currentShowTowerEntity.transform.position = hit.point;
                }
            }
        }

        public void ShowPreviewTower(TowerData towerData)
        {
            if (towerData == null)
                return;

            currentShowPreviewTower = towerData;
            uiMaskFormSerialId = GameEntry.UI.OpenUIForm(EnumUIForm.UIMask);
            entityLoader.ShowEntity<EntityAssaultCannonPreview>(towerData.EntityId, (entity) =>
             {
                 currentShowTowerEntity = entity;
             });
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
