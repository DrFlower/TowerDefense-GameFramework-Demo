using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine.UI;

namespace Flower
{
    public class UILevelMainInfoForm : UGuiForm
    {
        public Text hpText;
        public Text energyText;

        public Text waveText;
        public Image waveProgressImg;

        public Button btnStartWave;

        public Button btnPause;

        public Button debugAddEnergy;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            btnStartWave.onClick.AddListener(OnBtnStartWaveClick);
            btnPause.onClick.AddListener(OnPauseBtnClick);
            debugAddEnergy.onClick.AddListener(OnBtnAdEnrgyClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void UpdateHP()
        {

        }

        private void UpdateEnergy()
        {

        }

        private void UpdateWave()
        {

        }

        private void OnBtnStartWaveClick()
        {

        }

        private void OnPauseBtnClick()
        {
            GameEntry.UI.OpenUIForm(EnumUIForm.UIPausePanelForm);
        }

        private void OnBtnAdEnrgyClick()
        {

        }

    }
}

