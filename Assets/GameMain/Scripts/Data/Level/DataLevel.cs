using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace Flower
{
    public sealed class DataLevel : DataBase
    {
        private IDataTable<DRLevel> dtLevel;
        private Dictionary<int, LevelData> dicLevelData;

        private readonly static int NONE_LEVEL_INDEX = -1;

        public EnumLevelState LevelState
        {
            get;
            private set;
        }

        public int CurrentLevel
        {
            get;
            private set;
        }

        protected override void OnInit()
        {
            LevelState = EnumLevelState.None;
            CurrentLevel = NONE_LEVEL_INDEX;
        }

        protected override void OnPreload()
        {
            LoadDataTable("Level");
        }

        protected override void OnLoad()
        {
            dtLevel = GameEntry.DataTable.GetDataTable<DRLevel>();
            if (dtLevel == null)
                throw new System.Exception("Can not get data table Level");

            dicLevelData = new Dictionary<int, LevelData>();

            DRLevel[] dRLevels = dtLevel.GetAllDataRows();
            foreach (var dRLevel in dRLevels)
            {
                SceneData sceneData = GameEntry.Data.GetData<DataScene>().GetSceneData(dRLevel.SceneId);
                LevelData levelData = new LevelData(dRLevel, sceneData);
                dicLevelData.Add(dRLevel.Id, levelData);
            }
        }

        public LevelData GetLevelData(int id)
        {
            if (dicLevelData.ContainsKey(id))
            {
                return dicLevelData[id];
            }

            return null;
        }

        public LevelData[] GetAllLevelData()
        {
            int index = 0;
            LevelData[] results = new LevelData[dicLevelData.Count];
            foreach (var levelData in dicLevelData.Values)
            {
                results[index++] = levelData;
            }

            return results;
        }

        public void LoadLevel(int level)
        {
            if (LevelState == EnumLevelState.Loading)
            {
                Log.Error("Can not load level when loading level.");
                return;
            }

            if (level <= 0)
            {
                Log.Error("Load level param invaild '{0}.'", level);
                return;
            }

            if (!dicLevelData.ContainsKey(level))
            {
                Log.Error("Can not found level '{0}.'", level);
                return;
            }

            LevelData levelData = dicLevelData[level];

            if (level == CurrentLevel)
            {
                GameEntry.Event.Fire(this, ReloadLevelEventArgs.Create(levelData));
                return;
            }

            GameEntry.Event.Fire(this, LoadLevelEventArgs.Create(levelData));
        }

        public void LevelPause()
        {
            if (CurrentLevel == NONE_LEVEL_INDEX)
            {
                Log.Error("Only can pause in level");
                return;
            }

            if (LevelState != EnumLevelState.Normal)
            {
                Log.Error("Only can pause when level is in Normal State,now is {0}", LevelState.ToString());
                return;
            }

            LevelState = EnumLevelState.Pause;
        }

        public void LevelResume()
        {

        }



        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRLevel>();

            dtLevel = null;
            dicLevelData = null;

            LevelState = EnumLevelState.None;
            CurrentLevel = NONE_LEVEL_INDEX;
        }

        protected override void OnShutdown()
        {
        }
    }

}