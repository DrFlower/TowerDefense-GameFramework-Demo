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
        public Button btnPause;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            btnPause.onClick.AddListener(OnPauseBtnClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnPauseBtnClick()
        {
            GameEntry.UI.OpenUIForm(EnumUIForm.UIPausePanelForm);
        }

    }
}

