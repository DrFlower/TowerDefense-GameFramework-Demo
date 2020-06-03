using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower
{
    public class DataUIForm : DataBase
    {
        private IDataTable<DRUIForm> dtUIForm;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("UIForm");
        }

        protected override void OnLoad()
        {
            dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            if (dtUIForm == null)
                throw new System.Exception("Can not get data table UIForm");
        }

        public string GetAssetsPathByFormId(int formId)
        {
            DRUIForm drUIForm = dtUIForm.GetDataRow(formId);

            if (drUIForm == null)
            {
                throw new System.Exception(string.Format("Can nor find formId {0} from data table UIForm", formId));
            }

            return GameEntry.Data.GetData<DataAssetsPath>().GetAssetsPathByAssetsId(drUIForm.AssetId);
        }

        public DRUIForm GetDRUIFormByFormId(int formId)
        {
            DRUIForm drUIForm = dtUIForm.GetDataRow(formId);

            return drUIForm;
        }

        public DRUIForm[] GetAllUIFormDataRaw()
        {
            return dtUIForm.GetAllDataRows();
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRUIForm>();
        }

        protected override void OnShutdown()
        {
        }
    }

}

