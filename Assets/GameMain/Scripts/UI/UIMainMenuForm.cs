using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class UIMainMenuForm : UGuiFormEx
    {
        public Button levelSelectButton;
        public Button optionButton;
        public Button quitButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            levelSelectButton.onClick.AddListener(OnLevelSelectButtonClick);
            optionButton.onClick.AddListener(OnOptionButtonClick);
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnLevelSelectButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.UI.OpenUIForm(EnumUIForm.UILevelSelectForm);
        }

        private void OnOptionButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.UI.OpenUIForm(EnumUIForm.UIOptionsForm);
        }

        private void OnQuitButtonClick()
        {
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
        }

    }
}


