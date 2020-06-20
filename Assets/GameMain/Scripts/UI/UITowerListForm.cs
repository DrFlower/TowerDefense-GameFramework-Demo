using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine.UI;

namespace Flower
{
    public class UITowerListForm : UGuiForm
    {
        public Transform towerBuildButtonRoot;

        private Dictionary<int, GameObject> dicTowerId2Button;
        private Dictionary<int, int> dicSerialId2TowerId;

        private DataLevel dataLevel;
        private LevelData currentLevelData;

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
            currentLevelData = dataLevel.GetLevelData(dataLevel.CurrentLevel);
            if (currentLevelData == null)
                return;

            ShowTowerBuildButtons();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
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
                });
            }
        }


    }
}
