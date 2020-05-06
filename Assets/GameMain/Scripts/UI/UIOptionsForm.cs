using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class UIOptionsForm : UGuiForm
    {
        public Slider masterVolumeSlider;
        public Slider sFXVolumeSlider;
        public Slider musicVolumeSlider;

        public Toggle chineseSimplifiedToggle;
        public Toggle chineseTraditionalToggle;
        public Toggle englishToggle;

        public Button backButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backButton.onClick.AddListener(OnBackButtonClick);

            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChange);
            sFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderChange);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChange);

            chineseSimplifiedToggle.onValueChanged.AddListener(SelectChineseSimplifiedLanguage);
            chineseTraditionalToggle.onValueChanged.AddListener(SelectChineseTraditionalLanguage);
            englishToggle.onValueChanged.AddListener(SelectEnglishLanguage);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            masterVolumeSlider.value= GameEntry.Sound.GetVolume("Music");
            sFXVolumeSlider.value = GameEntry.Sound.GetVolume("SFX");
            musicVolumeSlider.value = GameEntry.Sound.GetVolume("UI");
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnBackButtonClick()
        {
            GameEntry.Sound.PlaySound(30007);
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
            GameEntry.Sound.SetVolume("UI", value);
        }

        private void SelectChineseSimplifiedLanguage(bool isOn)
        {

        }

        private void SelectChineseTraditionalLanguage(bool isOn)
        {

        }

        private void SelectEnglishLanguage(bool isOn)
        {

        }

    }
}


