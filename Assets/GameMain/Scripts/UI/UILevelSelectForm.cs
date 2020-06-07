using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System;
using GameFramework.Event;

namespace Flower
{
    public class UILevelSelectForm : UGuiForm
    {
        public Button backButton;
        public Transform levelSelectButtonRoot;
        private Dictionary<int, GameObject> dicLevelId2Button;
        private Dictionary<int, int> dicSerialId2LevelId;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backButton.onClick.AddListener(OnBackButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Subscribe(ShowItemSuccessEventArgs.EventId, OnShowItemSuccess);
            Subscribe(ShowItemFailureEventArgs.EventId, OnShowItemFail);

            ShowLevelSelectionButtonItems();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            foreach (var item in dicSerialId2LevelId.Keys)
            {
                GameEntry.Item.HideItem(item);
            }
        }

        private void OnBackButtonClick()
        {
            GameEntry.Sound.PlaySound(30007);
            Close();
        }

        private void ShowLevelSelectionButtonItems()
        {
            dicLevelId2Button = new Dictionary<int, GameObject>();
            dicSerialId2LevelId = new Dictionary<int, int>();

            LevelData[] levelDatas = GameEntry.Data.GetData<DataLevel>().GetAllLevelData();
            foreach (var levelData in levelDatas)
            {
                int serialId = GameEntry.Item.GenerateSerialId();
                dicSerialId2LevelId.Add(serialId, levelData.Id);
                GameEntry.Item.ShowItem<ItemLevelSelectionButton>(serialId, EnumItem.LevelSelectionButton, this);
            }
        }

        private void OnShowItemSuccess(object sender, GameEventArgs e)
        {
            ShowItemSuccessEventArgs ne = (ShowItemSuccessEventArgs)e;
            if ((object)ne.UserData != this)
            {
                return;
            }

            if (!dicSerialId2LevelId.ContainsKey(ne.Item.Id))
                return;

            int levelId = dicSerialId2LevelId[ne.Item.Id];

            dicLevelId2Button.Add(levelId, ne.Item.gameObject);

            ne.Item.transform.SetParent(levelSelectButtonRoot, false);
            ne.Item.transform.localScale = Vector3.one;
            ne.Item.transform.eulerAngles = Vector3.zero;
            ne.Item.transform.localPosition = Vector3.zero;

            ne.Item.GetComponent<ItemLevelSelectionButton>().SetLevelData(GameEntry.Data.GetData<DataLevel>().GetLevelData(levelId));
        }

        private void OnShowItemFail(object sender, GameEventArgs e)
        {
            ShowItemFailureEventArgs ne = (ShowItemFailureEventArgs)e;
            if ((object)ne.UserData != this)
            {
                return;
            }

            Log.Warning("Show item failure with error message '{0}'.", ne.ErrorMessage);
        }
    }

}

