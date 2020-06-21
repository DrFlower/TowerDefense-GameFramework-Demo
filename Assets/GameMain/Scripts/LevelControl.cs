using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    class LevelControl
    {
        private int? uiMaskFormSerialId;

        private TowerData currentShowPreviewTower;

        public void Enter()
        {
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
        }

        public void HidePreviewTower()
        {
            if (uiMaskFormSerialId != null)
                GameEntry.UI.CloseUIForm((int)uiMaskFormSerialId);

            GameEntry.Event.Fire(this, HidePreviewTowerEventArgs.Create(currentShowPreviewTower));
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
