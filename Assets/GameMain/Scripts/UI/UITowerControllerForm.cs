using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Flower.Data;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class UITowerControllerForm : UGuiFormEx
    {
        public Text towerNameText;
        public Text descriptionText;
        public Text dpsText;
        public Text upgradeDescriptionText;
        public Text upgradePriceText;
        public Text sellPriceText;

        public Button upgradeButton;
        public Button sellButton;
        public Button confirmSellButton;

        public Button maskButton;

        private Tower tower;

        private bool click = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
            sellButton.onClick.AddListener(OnSellButtonClick);
            confirmSellButton.onClick.AddListener(OnSellConfirmButtonClick);
            maskButton.onClick.AddListener(Close);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            tower = userData as Tower;
            if (tower == null)
            {
                Log.Error("Open UITowerConrollerForm Param inbaild");
                return;
            }

            towerNameText.text = tower.Name;
            descriptionText.text = tower.Des;
            dpsText.text = tower.DPS.ToString();
            upgradeDescriptionText.text = tower.UpgradeDes;

            DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();
            if (dataLevel.LevelState == EnumLevelState.Prepare)
                sellPriceText.text = tower.TotalCostEnergy.ToString();
            else
                sellPriceText.text = tower.SellEnergy.ToString();

            upgradeButton.gameObject.SetActive(!tower.IsMaxLevel);
            if (!tower.IsMaxLevel)
            {
                int upgradeNeedEngry = tower.GetBuildEnergy(tower.Level + 1);
                upgradePriceText.text = upgradeNeedEngry.ToString();

                DataPlayer dataPlayer = GameEntry.Data.GetData<DataPlayer>();
                upgradeButton.interactable = (dataPlayer.Energy >= upgradeNeedEngry);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            tower = null;
            confirmSellButton.gameObject.SetActive(false);
            click = false;
        }

        private void OnUpgradeButtonClick()
        {
            if (tower == null || click)
                return;

            DataTower dataTower = GameEntry.Data.GetData<DataTower>();
            dataTower.UpgradeTower(tower.SerialId);
            click = true;

            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            Close();
        }

        private void OnSellButtonClick()
        {
            if (tower == null)
                return;

            confirmSellButton.gameObject.SetActive(true);
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
        }

        private void OnSellConfirmButtonClick()
        {
            if (tower == null || click)
                return;

            DataTower dataTower = GameEntry.Data.GetData<DataTower>();
            dataTower.SellTower(tower.SerialId);
            click = true;
            GameEntry.Sound.PlaySound(EnumSound.TDTowerSell);
            Close();
        }

    }

}
