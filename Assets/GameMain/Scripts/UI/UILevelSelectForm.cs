using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System;
using GameFramework.Event;
using Flower.Data;

namespace Flower
{
    public class UILevelSelectForm : UGuiFormEx
    {
        public Button backButton;
        public Transform levelSelectButtonRoot;
        public MouseScroll mouseScroll;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backButton.onClick.AddListener(OnBackButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ShowLevelSelectionButtonItems();
            mouseScroll.Init();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnBackButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            Close();
        }

        private void ShowLevelSelectionButtonItems()
        {
            LevelData[] levelDatas = GameEntry.Data.GetData<DataLevel>().GetAllLevelData();
            foreach (var levelData in levelDatas)
            {
                ShowItem<ItemLevelSelectionButton>(EnumItem.LevelSelectionButton, (item) =>
                 {
                     item.transform.SetParent(levelSelectButtonRoot, false);
                     item.transform.localScale = Vector3.one;
                     item.transform.eulerAngles = Vector3.zero;
                     item.transform.localPosition = Vector3.zero;
                     item.GetComponent<ItemLevelSelectionButton>().SetLevelData(GameEntry.Data.GetData<DataLevel>().GetLevelData(levelData.Id));
                 });
            }
        }
    }

}

