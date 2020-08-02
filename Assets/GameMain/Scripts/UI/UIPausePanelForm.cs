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
            btnClose.onClick.AddListener(OnBtnClose);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();
            LevelData levelData = dataLevel.GetLevelData(dataLevel.CurrentLevelIndex);
            if (levelData == null)
            {
                Log.Error("Can not found level '{0}.'", dataLevel.CurrentLevelIndex);
                return;
            }

            levelTitleText.text = levelData.Name;
            levelDescriptionText.text = levelData.Description;

            dataLevel.LevelPause();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnBtnMainMenu()
        {
            GameEntry.Data.GetData<DataLevel>().ExitLevel();
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
        }

        private void OnBtnRestart()
        {
            int currentLevel = GameEntry.Data.GetData<DataLevel>().CurrentLevelIndex;
            GameEntry.Data.GetData<DataLevel>().LoadLevel(currentLevel);
            Close();
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
        }

        private void OnBtnClose()
        {
            DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();
            dataLevel.LevelResume();
            Close();
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
        }

    }

}

