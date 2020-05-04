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
        public Button backButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backButton.onClick.AddListener(OnBackButtonClick);
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChange);
            sFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderChange);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChange);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnBackButtonClick()
        {
            Close();
        }

        private void OnMasterVolumeSliderChange(float value)
        {

        }

        private void OnSFXVolumeSliderChange(float value)
        {

        }

        private void OnMusicVolumeSliderChange(float value)
        {

        }
    }
}


