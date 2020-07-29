using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System;
using Flower.Data;

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

        private Action<TowerData> onClick;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            buildButton.onClick.AddListener(OnBuildButtonClick);

            Subscribe(PlayerEnergyChangeEventArgs.EventId, OnPlayerEnergyChange);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            buildButton.onClick.RemoveAllListeners();
            towerData = null;
            towerLevelData = null;
            dataPlayer = null;
            this.onClick = null;
        }

        public void SetTowerBuildButton(TowerData towerData, Action<TowerData> onClick)
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

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            UpdateEnergyState(dataPlayer.Energy);

            this.onClick = onClick;
        }

        private void UpdateEnergyState(float ownEnergy)
        {
            if (towerLevelData == null)
                return;

            bool haveEnoughEnergy = ownEnergy >= towerLevelData.BuildEnergy;
            buildButton.interactable = haveEnoughEnergy;
            energyIcon.color = haveEnoughEnergy ? energyDefaultColor : energyInvalidColor;
        }

        public void OnBuildButtonClick()
        {
            if (onClick != null)
                onClick(towerData);
        }

        public void OnPlayerEnergyChange(object sender, GameEventArgs gameEventArgs)
        {
            PlayerEnergyChangeEventArgs ne = (PlayerEnergyChangeEventArgs)gameEventArgs;
            if (ne == null)
                return;

            UpdateEnergyState(ne.CurrentEnergy);
        }

    }
}


