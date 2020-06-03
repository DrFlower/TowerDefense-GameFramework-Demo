using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower
{
    public sealed class SoundData
    {
        public int id;
        public string assetPath;
        public SoundGroupData soundGroupData;
        public SoundPlayParam soundPlayParam;
    }

    public sealed class SoundGroupData
    {

    }

    public sealed class SoundPlayParam
    {

    }

    public class DataSound : DataBase
    {
        private IDataTable<DRSound> dtSound;
        private IDataTable<DRSoundGroup> dtSoundGroup;
        private IDataTable<DRSoundPlayParam> dtSoundPlayParam;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Sound");
            LoadDataTable("SoundGroup");
            LoadDataTable("SoundPlayParam");
        }

        protected override void OnLoad()
        {
            dtSound = GameEntry.DataTable.GetDataTable<DRSound>();
            if (dtSound == null)
                throw new System.Exception("Can not get data table Sound");

            dtSoundGroup = GameEntry.DataTable.GetDataTable<DRSoundGroup>();
            if (dtSoundGroup == null)
                throw new System.Exception("Can not get data table SoundGroup");

            dtSoundPlayParam = GameEntry.DataTable.GetDataTable<DRSoundPlayParam>();
            if (dtSoundPlayParam == null)
                throw new System.Exception("Can not get data table SoundPlayParam");
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRSound>();
            GameEntry.DataTable.DestroyDataTable<DRSoundGroup>();
            GameEntry.DataTable.DestroyDataTable<DRSoundPlayParam>();
        }

        protected override void OnShutdown()
        {
        }
    }

}