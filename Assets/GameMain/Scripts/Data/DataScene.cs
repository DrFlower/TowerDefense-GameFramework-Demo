using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    public sealed class SceneData
    {
        private DRScene dRScene;
        private DRAssetsPath dRAssetsPath;

        public int Id
        {
            get
            {
                return dRScene.Id;
            }
        }

        public string AssetPath
        {
            get
            {
                return dRAssetsPath.AssetPath;
            }
        }

        public string Procedure
        {
            get
            {
                return dRScene.Procedure;
            }
        }

        public SceneData(DRScene dRScene, DRAssetsPath dRAssetsPath)
        {
            this.dRScene = dRScene;
            this.dRAssetsPath = dRAssetsPath;
        }
    }

    public sealed class DataScene : DataBase
    {
        private IDataTable<DRScene> dtScene;
        private Dictionary<int, SceneData> dicSceneData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Scene");
        }

        protected override void OnLoad()
        {
            dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            if (dtScene == null)
                throw new System.Exception("Can not get data table Scene");

            dicSceneData = new Dictionary<int, SceneData>();

            DRScene[] dRScenes = dtScene.GetAllDataRows();
            foreach (var dRScene in dRScenes)
            {
                DRAssetsPath dRAssetsPath = GameEntry.Data.GetData<DataAssetsPath>().GetDRAssetsPathByAssetsId(dRScene.AssetId);
                SceneData sceneData = new SceneData(dRScene, dRAssetsPath);
                dicSceneData.Add(dRScene.Id, sceneData);
            }
        }

        public SceneData GetSceneData(int id)
        {
            if (dicSceneData.ContainsKey(id))
            {
                return dicSceneData[id];
            }

            return null;
        }

        public SceneData[] GetAllSceneData()
        {
            int index = 0;
            SceneData[] results = new SceneData[dicSceneData.Count];
            foreach (var sceneData in dicSceneData.Values)
            {
                results[index++] = sceneData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRScene>();

            dtScene = null;
            dicSceneData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}