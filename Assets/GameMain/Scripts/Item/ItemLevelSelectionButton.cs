using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using Flower.Data;
using GameFramework.Resource;

namespace Flower
{
    public class ItemLevelSelectionButton : ItemLogicEx
    {
        public GameObject mask;
        public Image downloadProgress;
        public Text progressText;
        public Text levelTitle;
        public Text levelDescription;
        public Button button;
        public Image[] stars;
        public CanvasGroup content;

        private LevelData levelData;

        private bool frameUpdate = false;
        private bool updateResourceGroup = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            frameUpdate = true;
        }

        public void SetLevelData(LevelData levelData)
        {
            this.levelData = levelData;

            levelTitle.text = levelData.Name;
            levelDescription.text = levelData.Description;
            button.onClick.AddListener(OnButtonClick);

            int currentStarCount = GameEntry.Setting.GetInt(string.Format(Constant.Setting.LevelStarRecord, levelData.Id), 0);
            for (int i = 0; i < stars.Length; i++)
            {

                stars[i].gameObject.SetActive(i < currentStarCount);
            }

            bool ready = GetResourceGroupIsReady();
            float progress = GetResourceGroupProgress();

            if (ready)
                SetDownFinishState();
            else
                SetNeedDownloadState(progress);

            if (ready)
                return;

            IResourceGroup resourceGroup = GetResourceGroup();
            IResourceGroup updatingResouceGroup = GameEntry.Resource.UpdatingResourceGroup;
            if (updatingResouceGroup != null && resourceGroup.Name == updatingResouceGroup.Name)
            {
                updateResourceGroup = true;
                UpdateDownloadProgress();
            }

            Subscribe(UnityGameFramework.Runtime.ResourceUpdateStartEventArgs.EventId, (sender, ge) => UpdateDownloadProgress());
            Subscribe(UnityGameFramework.Runtime.ResourceUpdateChangedEventArgs.EventId, (sender, ge) => UpdateDownloadProgress());
            Subscribe(UnityGameFramework.Runtime.ResourceUpdateSuccessEventArgs.EventId, (sender, ge) => UpdateDownloadProgress());
        }


        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            button.onClick.RemoveAllListeners();
            levelTitle.text = "";
            levelDescription.text = "";
            levelData = null;
            SetDownFinishState();

            updateResourceGroup = false;
        }

        private IResourceGroup GetResourceGroup()
        {
            if (GameEntry.Base.EditorResourceMode)
                return null;

            if (levelData == null)
                return null;

            string resouceGroupName = levelData.ResourceGroupName;
            IResourceGroup resourceGroup = GameEntry.Resource.GetResourceGroup(resouceGroupName);
            if (resourceGroup == null)
            {
                Log.Error("has no resource group '{0}',", resouceGroupName);
                return null;
            }

            return resourceGroup;
        }

        private bool GetResourceGroupIsReady()
        {
            if (GameEntry.Base.EditorResourceMode)
                return true;

            IResourceGroup resourceGroup = GetResourceGroup();
            if (resourceGroup == null)
            {
                return false;
            }

            return resourceGroup.Ready;
        }

        private float GetResourceGroupProgress()
        {
            if (GameEntry.Base.EditorResourceMode)
                return 1f;

            IResourceGroup resourceGroup = GetResourceGroup();
            if (resourceGroup == null)
            {
                return 0f;
            }

            return resourceGroup.Progress;
        }

        private void SetNeedDownloadState(float currentProgress = 0)
        {
            mask.SetActive(true);
            content.alpha = 0.2f;
            progressText.gameObject.SetActive(true);

            progressText.text = GameEntry.Localization.GetString("Download");

            downloadProgress.fillAmount = currentProgress;
        }

        private void SetDownFinishState()
        {
            mask.SetActive(false);
            content.alpha = 1;
            progressText.gameObject.SetActive(false);
            progressText.text = "";
            downloadProgress.fillAmount = 1;
        }

        private void OnButtonClick()
        {
            if (levelData == null)
                return;
   
            if (!GetResourceGroupIsReady())
            {
                IResourceGroup resourceGroup = GetResourceGroup();
                IResourceGroup updatingResouceGroup = GameEntry.Resource.UpdatingResourceGroup;
                if (updatingResouceGroup != null && resourceGroup.Name != updatingResouceGroup.Name)
                {
                    Log.Error(string.Format("There is already a resource group '{0}' being updated.", updatingResouceGroup.Name));
                    return;
                }

                if (!updateResourceGroup)
                {
                    updateResourceGroup = true;
                    GameEntry.Resource.UpdateResources(levelData.ResourceGroupName, OnUpdateResourcesComplete);
                }

                return;
            }

            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Data.GetData<DataLevel>().LoadLevel(levelData.Id);
        }

        private void UpdateDownloadProgress()
        {
            if (GameEntry.Base.EditorResourceMode)
                return;

            if (!frameUpdate)
                return;

            float progress = GetResourceGroupProgress();

            downloadProgress.fillAmount = progress;

            if (updateResourceGroup)
                progressText.text = string.Format("{0:N2}% ", progress * 100);

            frameUpdate = false;
        }

        private void OnUpdateResourcesComplete(IResourceGroup resourceGroup, bool result)
        {
            //这里可能下载完的时候这个UI已经被销毁了，做个gameObject判空
            if (gameObject == null || levelData == null)
                return;

            if (resourceGroup.Name != levelData.ResourceGroupName)
            {
                if (!GetResourceGroupIsReady())
                    progressText.text = GameEntry.Localization.GetString("Download");

                return;
            }

            if (result)
            {
                SetDownFinishState();
                Log.Info("Update resources complete with no errors.");
            }
            else
            {
                Log.Error("Update resources complete with errors.");
            }
        }

    }
}


