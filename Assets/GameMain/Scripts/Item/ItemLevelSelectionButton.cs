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
        private Text levelTitle;
        private Text levelDescription;
        private Button button;
        private Image[] stars;

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


