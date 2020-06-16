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

        public GameObject debugAddEnergyPanel;
        public Button debugAddEnergyBtton;
        public Text debugAddEnergyText;

        private DataPlayer dataPlayer;
        private DataLevel dataLevel;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            btnStartWave.onClick.AddListener(OnBtnStartWaveClick);
            btnPause.onClick.AddListener(OnPauseBtnClick);
            debugAddEnergyBtton.onClick.AddListener(OnBtnAdEnrgyClick);

            debugAddEnergyPanel.gameObject.SetActive(dataPlayer.IsEnableDebugEnergy);
            debugAddEnergyText.text = dataPlayer.DebugAddEnergyCount.ToString();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            Subscribe(PlayerHPChangeEventArgs.EventId, OnPlayerHPChange);
            Subscribe(PlayerEnergyChangeEventArgs.EventId, OnPlayerEnergyChange);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataLevel = GameEntry.Data.GetData<DataLevel>();

            hpText.text = dataPlayer.HP.ToString();
            energyText.text = dataPlayer.Energy.ToString();


        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            dataPlayer = null;
            dataLevel = null;
        }

        private void OnPlayerHPChange(object sender, GameEventArgs e)
        {
            PlayerHPChangeEventArgs ne = (PlayerHPChangeEventArgs)e;
            if (ne == null)
                return;

            hpText.text = ne.CurrentHP.ToString();
        }

        private void OnPlayerEnergyChange(object sender, GameEventArgs e)
        {
            PlayerEnergyChangeEventArgs ne = (PlayerEnergyChangeEventArgs)e;
            if (ne == null)
                return;

            energyText.text = ne.CurrentEnergy.ToString();
        }

        private void OnWaveUpdate(object sender, GameEventArgs e)
        {

        }

        private void OnBtnStartWaveClick()
        {
            dataLevel.StartWave();
        }

        private void OnPauseBtnClick()
        {
            GameEntry.UI.OpenUIForm(EnumUIForm.UIPausePanelForm);
        }

        private void OnBtnAdEnrgyClick()
        {
            dataPlayer.DebugAddEnergy();
        }

    }
}

