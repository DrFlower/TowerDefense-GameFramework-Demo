using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower
{
    public sealed class LevelData
    {
        private DRLevel dRLevel;
        private SceneData sceneData;
        private string name;
        private string description;

        public int Id
        {
            get
            {
                return dRLevel.Id;
            }
        }

        public string Name
        {
            get
            {
                return GameEntry.Localization.GetString(dRLevel.NameId);
            }
        }

        public string Description
        {
            get
            {
                return GameEntry.Localization.GetString(dRLevel.DescriptionId);
            }
        }

        public SceneData SceneData
        {
            get
            {
                return sceneData;
            }
        }

        public LevelData(DRLevel dRLevel, SceneData sceneData)
        {
            this.dRLevel = dRLevel;
            this.sceneData = sceneData;
        }
    }

    public sealed class DataLevel : DataBase
    {
        private IDataTable<DRLevel> dtLevel;
        private Dictionary<int, LevelData> dicLevelData;

        protected override void OnInit()
        {

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

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRLevel>();

            dtLevel = null;
            dicLevelData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}