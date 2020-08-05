using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine.UI;
using Flower.Data;
using GameFramework;

namespace Flower
{
    public class UIGameOverForm : UGuiFormEx
    {
        public Text title;
        public Image[] starImages;
        public Sprite starBgSprite;
        public Sprite starSprite;
        public Button nextLevelButton;
        public Button mainMenuButton;
        public Button restartButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            UIGameOverFormOpenParam uIGameOverFormOpenParam = userData as UIGameOverFormOpenParam;
            if (uIGameOverFormOpenParam == null)
            {
                Log.Error("UIGameOverForm open param tyoe invaild");
                return;
            }

            switch (uIGameOverFormOpenParam.EnumGameOverType)
            {
                case EnumGameOverType.Success:
                    title.text = string.Format(GameEntry.Localization.GetString("Level Complete"), uIGameOverFormOpenParam.LevelData.Name);
                    GameEntry.Sound.PlaySound(EnumSound.TDVictory);
                    break;
                case EnumGameOverType.Fail:
                    title.text = string.Format(GameEntry.Localization.GetString("Level Failed"), uIGameOverFormOpenParam.LevelData.Name);
                    GameEntry.Sound.PlaySound(EnumSound.TDDefeat);
                    break;
            }

            for (int i = 0; i < starImages.Length; i++)
            {
                if (i < uIGameOverFormOpenParam.StarCount)

                    starImages[i].sprite = starSprite;
                else
                    starImages[i].sprite = starBgSprite;
            }

            nextLevelButton.gameObject.SetActive(uIGameOverFormOpenParam.EnumGameOverType == EnumGameOverType.Success);
            DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();
            int nextLevel = dataLevel.CurrentLevelIndex + 1;
            nextLevelButton.interactable = nextLevel <= dataLevel.MaxLevel;

            ReferencePool.Release(uIGameOverFormOpenParam);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        public void OnNextLevelButtonClick()
        {
            DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();
            int nextLevel = dataLevel.CurrentLevelIndex + 1;
            if (nextLevel <= dataLevel.MaxLevel)
                dataLevel.LoadLevel(nextLevel);
        }

        public void OnMainMenuButtonClick()
        {
            GameEntry.Data.GetData<DataLevel>().ExitLevel();
        }

        public void OnRestartButtonClick()
        {
            int currentLevel = GameEntry.Data.GetData<DataLevel>().CurrentLevelIndex;
            GameEntry.Data.GetData<DataLevel>().LoadLevel(currentLevel);
            Close();
        }
    }
}
