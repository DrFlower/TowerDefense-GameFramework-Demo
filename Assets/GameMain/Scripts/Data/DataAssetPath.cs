using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower
{
    public class DataAssetsPath : DataBase
    {
        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("AssetsPath");
        }

        protected override void OnLoad()
        {
        }

        public IDataTable<DRAssetsPath> GetAssetsPathDataTable()
        {
            return GameEntry.DataTable.GetDataTable<DRAssetsPath>();
        }

        public string GetAssetsPathByAssetsId(int assetId)
        {
            IDataTable<DRAssetsPath> dtAssetPath = GameEntry.DataTable.GetDataTable<DRAssetsPath>();
            DRAssetsPath drAssetPath = dtAssetPath.GetDataRow(assetId);

            if (drAssetPath == null)
            {
                throw new System.Exception(string.Format("Can nor find assetId {0} from data table AssetsPath", assetId));
            }

            return drAssetPath.AssetPath;
        }

        public DRAssetsPath[] GetAllAssetsPathDataRaw()
        {
            IDataTable<DRAssetsPath> dtAssetPath = GameEntry.DataTable.GetDataTable<DRAssetsPath>();

            return dtAssetPath.GetAllDataRows();
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRAssetsPath>();
        }

        protected override void OnShutdown()
        {

        }
    }

}