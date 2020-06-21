using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace Flower
{
    public class ItemTowerBuildButton : ItemLogicEx
    {
        public Text energyText;

        public Image towerIcon;

        public Button buildButton;

        public Image energyIcon;

        public Color energyDefaultColor;

        public Color energyInvalidColor;

        public Sprite[] iconList;

        private TowerData towerData;
        private TowerLevelData towerLevelData;
        private DataPlayer dataPlayer;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            buildButton.onClick.AddListener(OnBuildButtonClick);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            UpdateEnergyIconColor(dataPlayer.Energy);

            Subscribe(PlayerEnergyChangeEventArgs.EventId, OnPlayerEnergyChange);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            buildButton.onClick.RemoveAllListeners();
            towerData = null;
            towerLevelData = null;
            dataPlayer = null;
        }

        public void SetTowerBuildButton(TowerData towerData)
        {
            if (towerData == null)
                return;

            this.towerData = towerData;

            towerLevelData = towerData.GetTowerLevelData(0);

            energyText.text = towerLevelData.BuildEnergy.ToString();
            foreach (var item in iconList)
            {
                if (towerData.Icon == item.name)
                    towerIcon.sprite = item;
            }
        }

        private void UpdateEnergyIconColor(int ownEnergy)
        {
            if (towerLevelData == null)
                return;

            if (ownEnergy >= towerLevelData.BuildEnergy)
                energyIcon.color = energyDefaultColor;
            else
                energyIcon.color = energyInvalidColor;
        }

        public void OnBuildButtonClick()
        {

        }

        public void OnPlayerEnergyChange(object sender, GameEventArgs gameEventArgs)
        {
            PlayerEnergyChangeEventArgs ne = (PlayerEnergyChangeEventArgs)gameEventArgs;
            if (ne == null)
                return;

            UpdateEnergyIconColor(ne.CurrentEnergy);
        }

    }
}


