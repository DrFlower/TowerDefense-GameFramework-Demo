using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine.UI;

namespace Flower
{
    public class UIPausePanelForm : UGuiForm
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
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Menu")));
        }

        private void OnBtnRestart()
        {

        }

    }

}

