using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    public sealed class DataAssetsPath : DataBase
    {
        private IDataTable<DRAssetsPath> dtAssetPath;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("AssetsPath");
        }

        protected override void OnLoad()
        {
            dtAssetPath = GameEntry.DataTable.GetDataTable<DRAssetsPath>();

            if (dtAssetPath == null)
                throw new System.Exception("Can not get data table AssetsPath");
        }

        public IDataTable<DRAssetsPath> GetAssetsPathDataTable()
        {
            return GameEntry.DataTable.GetDataTable<DRAssetsPath>();
        }

        public DRAssetsPath GetDRAssetsPathByAssetsId(int assetId)
        {
            DRAssetsPath drAssetPath = dtAssetPath.GetDataRow(assetId);

            if (drAssetPath == null)
            {
                throw new System.Exception(string.Format("Can not find assetId {0} from data table AssetsPath", assetId));
            }

            return drAssetPath;
        }

        public string GetAssetsPathByAssetsId(int assetId)
        {
            DRAssetsPath drAssetPath = dtAssetPath.GetDataRow(assetId);

            if (drAssetPath == null)
            {
                throw new System.Exception(string.Format("Can not find assetId {0} from data table AssetsPath", assetId));
            }

            return drAssetPath.AssetPath;
        }

        public DRAssetsPath[] GetAllAssetsPathDataRaw()
        {
            return dtAssetPath.GetAllDataRows();
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRAssetsPath>();

            dtAssetPath = null;
        }

        protected override void OnShutdown()
        {

        }
    }

}