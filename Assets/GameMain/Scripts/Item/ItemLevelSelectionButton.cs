using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using Flower.Data;

namespace Flower
{
    public class ItemLevelSelectionButton : ItemLogicEx
    {
        public Text levelTitle;
        public Text levelDescription;
        public Button button;
        public Image[] stars;

        private LevelData levelData;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            levelTitle = transform.Find("LevelName").GetComponent<Text>();
            levelDescription = transform.Find("Description").GetComponent<Text>();
            button = gameObject.GetComponent<Button>();
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

        }

        protected override void OnRecycle()
        {
            base.OnRecycle();

            button.onClick.RemoveAllListeners();
            levelTitle.text = "";
            levelDescription.text = "";
        }

        private void OnButtonClick()
        {
            GameEntry.Sound.PlaySound(30008);

            if (levelData == null)
                return;

            GameEntry.Data.GetData<DataLevel>().LoadLevel(levelData.Id);
        }

    }
}


