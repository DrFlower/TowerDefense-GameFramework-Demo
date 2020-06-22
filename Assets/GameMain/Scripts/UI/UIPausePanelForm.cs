using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine.UI;
using Flower.Data;

namespace Flower
{
    public class UIPausePanelForm : UGuiFormEx
    {
        public Text levelTitleText;
        public Text levelDescriptionText;

        public Button btnMainMenu;
        public Button btnRestart;
        public Button btnClose;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            btnMainMenu.onClick.AddListener(OnBtnMainMenu);
            btnRestart.onClick.AddListener(OnBtnRestart);
            btnClose.onClick.AddListener(Close);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            //todo set text by level data
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnBtnMainMenu()
        {
            GameEntry.Data.GetData<DataLevel>().ExitLevel();
        }

        private void OnBtnRestart()
        {
            int currentLevel = GameEntry.Data.GetData<DataLevel>().CurrentLevel;
            GameEntry.Data.GetData<DataLevel>().LoadLevel(currentLevel);
        }

    }

}

