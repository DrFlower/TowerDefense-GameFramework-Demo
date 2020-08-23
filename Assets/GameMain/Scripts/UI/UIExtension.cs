//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using GameFramework.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using Flower.Data;

namespace Flower
{
    public static class UIExtension
    {
        private static int? downloadFormId = null;

        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
        {
            float time = 0f;
            float originalValue = slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }

        public static bool HasUIForm(this UIComponent uiComponent, EnumUIForm uiFormId, string uiGroupName = null)
        {
            return uiComponent.HasUIForm((int)uiFormId, uiGroupName);
        }

        public static bool HasUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            UIData uiData = GameEntry.Data.GetData<DataUI>().GetUIData(uiFormId);

            if (uiData == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(uiGroupName))
            {
                return uiComponent.HasUIForm(uiData.AssetPath);
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return false;
            }

            return uiGroup.HasUIForm(uiData.AssetPath);
        }

        public static UGuiForm GetUIForm(this UIComponent uiComponent, EnumUIForm uiFormId, string uiGroupName = null)
        {
            return uiComponent.GetUIForm((int)uiFormId, uiGroupName);
        }

        public static UGuiForm GetUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            UIData uiData = GameEntry.Data.GetData<DataUI>().GetUIData(uiFormId);
            if (uiData == null)
            {
                return null;
            }

            UIForm uiForm = null;
            if (string.IsNullOrEmpty(uiGroupName))
            {
                uiForm = uiComponent.GetUIForm(uiData.AssetPath);
                if (uiForm == null)
                {
                    return null;
                }

                return (UGuiForm)uiForm.Logic;
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return null;
            }

            uiForm = (UIForm)uiGroup.GetUIForm(uiData.AssetPath);
            if (uiForm == null)
            {
                return null;
            }

            return (UGuiForm)uiForm.Logic;
        }

        public static void OpenDownloadForm(this UIComponent uiComponent, object userData = null)
        {
            if (downloadFormId == null)
                downloadFormId = uiComponent.OpenUIForm(EnumUIForm.UIDownloadForm, userData);
        }

        public static void CloseDownloadForm(this UIComponent uiComponent)
        {
            if (downloadFormId != null)
            {
                UIForm uIForm = uiComponent.GetUIForm((int)downloadFormId);
                if (uIForm != null)
                    uiComponent.CloseUIForm(uIForm);
                downloadFormId = null;
            }
        }

        public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, EnumUIForm uiFormId, object userData = null)
        {
            return uiComponent.OpenUIForm((int)uiFormId, userData);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            UIData uiData = GameEntry.Data.GetData<DataUI>().GetUIData(uiFormId);
            if (uiData == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = uiData.AssetPath;
            if (!uiData.AllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }

            return uiComponent.OpenUIForm(assetName, uiData.UIGroupData.Name, Constant.AssetPriority.UIFormAsset, uiData.PauseCoveredUIForm, userData);
        }
    }
}
