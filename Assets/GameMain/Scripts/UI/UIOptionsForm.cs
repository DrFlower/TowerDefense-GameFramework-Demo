using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameFramework.Localization;

namespace Flower
{
    public class UIOptionsForm : UGuiFormEx
    {
        [System.Serializable]
        public class ToggleLanguage
        {
            public Language language;
            public Toggle toggle;
        }


        public Slider masterVolumeSlider;
        public Slider sFXVolumeSlider;
        public Slider musicVolumeSlider;

        public ToggleLanguage[] languageToggle;

        public Button backButton;

        public GameObject tipsGO;
        public Button confirmButton;
        public Button cancelButton;

        private Language currentLanguage;
        private Language selectLanguage;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backButton.onClick.AddListener(OnBackButtonClick);

            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChange);
            sFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderChange);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChange);

            foreach (var item in languageToggle)
            {
                item.toggle.onValueChanged.AddListener((isOn) => OnLanguageToggleChange(isOn, item.language));
            }

            confirmButton.onClick.AddListener(OnConfirmButtonClick);
            cancelButton.onClick.AddListener(OnCancelButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            masterVolumeSlider.value = GameEntry.Sound.GetVolume("Music");
            sFXVolumeSlider.value = GameEntry.Sound.GetVolume("SFX");
            musicVolumeSlider.value = GameEntry.Sound.GetVolume("SFX/UI");

            currentLanguage = (Language)GameEntry.Setting.GetInt(Constant.Setting.Language, (int)Language.English);
            selectLanguage = currentLanguage;

            foreach (var item in languageToggle)
            {
                item.toggle.isOn = item.language == currentLanguage;
            }

            tipsGO.SetActive(false);
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

        private void OnMasterVolumeSliderChange(float value)
        {
            GameEntry.Sound.SetVolume("Music", value);
        }

        private void OnSFXVolumeSliderChange(float value)
        {
            GameEntry.Sound.SetVolume("SFX", value);
        }

        private void OnMusicVolumeSliderChange(float value)
        {
            GameEntry.Sound.SetVolume("SFX/UI", value);
        }


        private void OnLanguageToggleChange(bool isOn, Language language)
        {
            if (!isOn)
                return;

            selectLanguage = language;
            tipsGO.SetActive(language != currentLanguage);
        }

        private void OnConfirmButtonClick()
        {
            GameEntry.Setting.SetInt(Constant.Setting.Language, (int)selectLanguage);
            GameEntry.Setting.Save();

            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }

        private void OnCancelButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            CancelLanguageChange();
            tipsGO.SetActive(false);
        }

        private void CancelLanguageChange()
        {
            foreach (var item in languageToggle)
            {
                item.toggle.isOn = item.language == currentLanguage;
            }

            selectLanguage = currentLanguage;
        }

    }
}


