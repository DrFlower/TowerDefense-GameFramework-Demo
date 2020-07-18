using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine.UI;
using Flower.Data;

namespace Flower
{
    public class UITowerListForm : UGuiFormEx
    {
        public Transform towerBuildButtonRoot;
        public RectTransform buildInfo;
        public Text buildInfoName;
        public Text buildInfoDps;
        public Text BuildInfoDes;
        public float BuildInfoFadeSpeed;

        private Dictionary<int, GameObject> dicTowerId2Button;
        private Dictionary<int, int> dicSerialId2TowerId;

        private DataLevel dataLevel;
        private LevelData currentLevelData;
        private DataTower dataTower;

        private bool showBuildInfo = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dicTowerId2Button = new Dictionary<int, GameObject>();
            dicSerialId2TowerId = new Dictionary<int, int>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            dataLevel = GameEntry.Data.GetData<DataLevel>();
            if (dataLevel == null)
                return;

            currentLevelData = dataLevel.GetLevelData(dataLevel.CurrentLevelIndex);
            if (currentLevelData == null)
                return;

            dataTower = GameEntry.Data.GetData<DataTower>();
            if (dataTower == null)
                return;

            ShowTowerBuildButtons();
            buildInfo.anchoredPosition = new Vector2(buildInfo.anchoredPosition.x, -200);
            showBuildInfo = false;

            Subscribe(HidePreviewTowerEventArgs.EventId, OnHidePreviewTower);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            float targetPosY = showBuildInfo ? 0 : -200;

            buildInfo.anchoredPosition = new Vector2(buildInfo.anchoredPosition.x, Mathf.Lerp(buildInfo.anchoredPosition.y, targetPosY, realElapseSeconds * BuildInfoFadeSpeed));

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            buildInfoName.text = string.Empty;
            buildInfoDps.text = string.Empty;
            BuildInfoDes.text = string.Empty;
        }

        private void ShowTowerBuildButtons()
        {
            dicTowerId2Button.Clear();
            dicSerialId2TowerId.Clear();

            int[] allowTowers = currentLevelData.AllowTowers;
            foreach (var towers in allowTowers)
            {
                ShowItem<ItemTowerBuildButton>(EnumItem.TowerBuildButton, (item) =>
                {
                    item.transform.SetParent(towerBuildButtonRoot, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero;

                    ItemTowerBuildButton itemTowerBuildButton = item.GetComponent<ItemTowerBuildButton>();
                    TowerData towerData = dataTower.GetTowerData(towers);
                    itemTowerBuildButton.SetTowerBuildButton(towerData, ShowBuildInfo);
                });
            }
        }

        public void ShowBuildInfo(TowerData towerData)
        {
            if (towerData == null)
                return;

            TowerLevelData towerLevelData = towerData.GetTowerLevelData(0);
            if (towerLevelData == null)
                return;

            buildInfoName.text = towerData.Name;
            buildInfoDps.text = towerLevelData.DPS.ToString();
            BuildInfoDes.text = towerLevelData.Des;

            GameEntry.Event.Fire(this, ShowPreviewTowerEventArgs.Create(towerData));

            showBuildInfo = true;
        }

        private void HideBuildInfo()
        {
            showBuildInfo = false;
        }

        private void OnHidePreviewTower(object sender, GameEventArgs e)
        {
            HidePreviewTowerEventArgs ne = (HidePreviewTowerEventArgs)e;
            if (ne == null)
                return;

            HideBuildInfo();
        }

    }
}
