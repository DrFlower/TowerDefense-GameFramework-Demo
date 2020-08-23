using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine.UI;
using GameFramework;

namespace Flower
{
    public class UIDownloadForm : UGuiFormEx
    {
        public GameObject Content;
        public Text speedText;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (GameEntry.Download.WorkingAgentCount > 0)
            {
                if (!Content.activeSelf)
                    Content.SetActive(true);

                speedText.text = GetByteLengthString((int)GameEntry.Download.CurrentSpeed);
            }
            else
            {
                if (Content.activeSelf)
                    Content.SetActive(false);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private string GetByteLengthString(long byteLength)
        {
            if (byteLength < 1024L) // 2 ^ 10
            {
                return Utility.Text.Format("{0} Bytes/s", byteLength.ToString());
            }

            if (byteLength < 1048576L) // 2 ^ 20
            {
                return Utility.Text.Format("{0} KB/s", (byteLength / 1024f).ToString("F2"));
            }

            if (byteLength < 1073741824L) // 2 ^ 30
            {
                return Utility.Text.Format("{0} MB/s", (byteLength / 1048576f).ToString("F2"));
            }

            if (byteLength < 1099511627776L) // 2 ^ 40
            {
                return Utility.Text.Format("{0} GB/s", (byteLength / 1073741824f).ToString("F2"));
            }

            if (byteLength < 1125899906842624L) // 2 ^ 50
            {
                return Utility.Text.Format("{0} TB/s", (byteLength / 1099511627776f).ToString("F2"));
            }

            if (byteLength < 1152921504606846976L) // 2 ^ 60
            {
                return Utility.Text.Format("{0} PB/s", (byteLength / 1125899906842624f).ToString("F2"));
            }

            return Utility.Text.Format("{0} EB/s", (byteLength / 1152921504606846976f).ToString("F2"));
        }

    }

}