using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine.UI;
using Flower.Data;

namespace Flower
{
    public class UILevelMainInfoForm : UGuiFormEx
    {
        public Text hpText;
        public Text energyText;

        public Button debugAddEnergyBtton;
        public Text debugAddEnergyText;

        public GameObject waveInfoPanel;
        public Button btnStartWave;
        public Text waveText;
        public Image waveProgressImg;

        public Button btnPause;

        private DataPlayer dataPlayer;
        private DataLevel dataLevel;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            btnStartWave.onClick.AddListener(OnBtnStartWaveClick);
            btnPause.onClick.AddListener(OnPauseBtnClick);
            debugAddEnergyBtton.onClick.AddListener(OnBtnAdEnrgyClick);

            Subscribe(PlayerHPChangeEventArgs.EventId, OnPlayerHPChange);
            Subscribe(PlayerEnergyChangeEventArgs.EventId, OnPlayerEnergyChange);
            Subscribe(LevelStateChangeEventArgs.EventId, OnLevelStateChange);
            Subscribe(WaveInfoUpdateEventArgs.EventId, OnWaveUpdate);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataLevel = GameEntry.Data.GetData<DataLevel>();

            debugAddEnergyBtton.gameObject.SetActive(dataPlayer.IsEnableDebugEnergy);
            debugAddEnergyText.text = dataPlayer.DebugAddEnergyCount.ToString();

            hpText.text = dataPlayer.HP.ToString();
            energyText.text = dataPlayer.Energy.ToString();

            btnStartWave.gameObject.SetActive(true);
            waveInfoPanel.SetActive(false);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            btnStartWave.onClick.RemoveAllListeners();
            btnPause.onClick.RemoveAllListeners();
            debugAddEnergyBtton.onClick.RemoveAllListeners();

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

        private void OnLevelStateChange(object sender, GameEventArgs e)
        {
            LevelStateChangeEventArgs ne = (LevelStateChangeEventArgs)e;
            if (ne == null)
                return;

            if (ne.CurrentState == EnumLevelState.Normal)
            {
                btnStartWave.gameObject.SetActive(false);
                waveInfoPanel.SetActive(true);
            }
            else if (ne.CurrentState == EnumLevelState.Prepare)
            {
                btnStartWave.gameObject.SetActive(true);
                waveInfoPanel.SetActive(false);
            }
        }

        private void OnWaveUpdate(object sender, GameEventArgs e)
        {
            WaveInfoUpdateEventArgs ne = (WaveInfoUpdateEventArgs)e;
            if (ne == null)
                return;

            waveProgressImg.fillAmount = ne.CurrentWaveProgress;

            waveText.text = string.Format("{0}/{1}", ne.CurrentWave, ne.TotalWave);
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

