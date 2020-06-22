using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    #region Data

    public sealed class SoundData
    {
        private DRSound dRSound;
        private DRAssetsPath dRAssetsPath;

        public int id
        {
            get
            {
                return dRSound.Id;
            }
        }
        public string AssetPath
        {
            get
            {
                return dRAssetsPath.AssetPath;
            }
        }
        public SoundGroupData SoundGroupData
        {
            get;
            private set;
        }

        public SoundPlayParamData SoundPlayParam
        {
            get;
            private set;
        }

        public SoundData(DRSound dRSound, DRAssetsPath dRAssetsPath, SoundGroupData soundGroup, SoundPlayParamData soundPlayParam)
        {
            this.dRSound = dRSound;
            this.dRAssetsPath = dRAssetsPath;
            this.SoundGroupData = soundGroup;
            this.SoundPlayParam = soundPlayParam;
        }
    }

    public sealed class SoundGroupData
    {
        private DRSoundGroup dRSoundGroup;

        public int Id
        {
            get
            {
                return dRSoundGroup.Id;
            }
        }

        public string Name
        {
            get
            {
                return dRSoundGroup.Name;
            }
        }

        public int SoundAgentCount
        {
            get
            {
                return dRSoundGroup.SoundAgentCount;
            }
        }


        public bool AvoidBeingReplacedBySamePriority
        {
            get
            {
                return dRSoundGroup.AvoidBeingReplacedBySamePriority;
            }
        }

        public bool Mute
        {
            get
            {
                return dRSoundGroup.Mute;
            }
        }

        public float Volume
        {
            get
            {
                return dRSoundGroup.Volume;
            }
        }

        public SoundGroupData(DRSoundGroup dRSoundGroup)
        {
            this.dRSoundGroup = dRSoundGroup;
        }
    }

    public sealed class SoundPlayParamData
    {
        private DRSoundPlayParam dRSoundPlayParam;

        public int Id
        {
            get
            {
                return dRSoundPlayParam.Id;
            }
        }

        public float Time
        {
            get
            {
                return dRSoundPlayParam.Time;
            }
        }

        public bool Mute
        {
            get
            {
                return dRSoundPlayParam.Mute;
            }
        }

        public bool Loop
        {
            get
            {
                return dRSoundPlayParam.Loop;
            }
        }

        public int Priority
        {
            get
            {
                return dRSoundPlayParam.Priority;
            }
        }

        public float Volume
        {
            get
            {
                return dRSoundPlayParam.Volume;
            }
        }

        public float FadeInSeconds
        {
            get
            {
                return dRSoundPlayParam.FadeInSeconds;
            }
        }

        public float Pitch
        {
            get
            {
                return dRSoundPlayParam.Pitch;
            }
        }

        public float PanStereo
        {
            get
            {
                return dRSoundPlayParam.PanStereo;
            }
        }

        public float SpatialBlend
        {
            get
            {
                return dRSoundPlayParam.SpatialBlend;
            }
        }

        public float MaxDistance
        {
            get
            {
                return dRSoundPlayParam.MaxDistance;
            }
        }

        public float DopplerLevel
        {
            get
            {
                return dRSoundPlayParam.DopplerLevel;
            }
        }

        public SoundPlayParamData(DRSoundPlayParam dRSoundPlayParam)
        {
            this.dRSoundPlayParam = dRSoundPlayParam;
        }
    }

    #endregion

    public sealed class DataSound : DataBase
    {
        private IDataTable<DRSound> dtSound;
        private IDataTable<DRSoundGroup> dtSoundGroup;
        private IDataTable<DRSoundPlayParam> dtSoundPlayParam;

        private Dictionary<int, SoundData> dicSoundData;
        private Dictionary<int, SoundGroupData> dicSoundGroupData;
        private Dictionary<int, SoundPlayParamData> dicSoundPlayParamData;

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

            dicSoundData = new Dictionary<int, SoundData>();
            dicSoundGroupData = new Dictionary<int, SoundGroupData>();
            dicSoundPlayParamData = new Dictionary<int, SoundPlayParamData>();

            DRSound[] dRSounds = dtSound.GetAllDataRows();
            foreach (var dRSound in dRSounds)
            {
                SoundGroupData soundGroupData = null;
                if (!dicSoundGroupData.TryGetValue(dRSound.SoundGroupId, out soundGroupData))
                {
                    DRSoundGroup dRSoundGroup = dtSoundGroup.GetDataRow(dRSound.SoundGroupId);
                    if (dRSoundGroup == null)
                    {
                        throw new System.Exception("Can not find SoundGroup id :" + dRSound.SoundGroupId);
                    }
                    soundGroupData = new SoundGroupData(dRSoundGroup);
                    dicSoundGroupData.Add(dRSound.SoundGroupId, soundGroupData);
                }

                SoundPlayParamData soundPlayParamData = null;
                if (!dicSoundPlayParamData.TryGetValue(dRSound.SoundPlayParamId, out soundPlayParamData))
                {
                    DRSoundPlayParam dRSoundPlayParam = dtSoundPlayParam.GetDataRow(dRSound.SoundPlayParamId);
                    if (dRSoundPlayParam == null)
                    {
                        throw new System.Exception("Can not find SoundPlayParam id :" + dRSound.SoundPlayParamId);
                    }
                    soundPlayParamData = new SoundPlayParamData(dRSoundPlayParam);
                    dicSoundPlayParamData.Add(dRSound.SoundPlayParamId, soundPlayParamData);
                }

                DRAssetsPath dRAssetsPath = GameEntry.Data.GetData<DataAssetsPath>().GetDRAssetsPathByAssetsId(dRSound.AssetId);

                SoundData soundData = new SoundData(dRSound, dRAssetsPath, soundGroupData, soundPlayParamData);
                dicSoundData.Add(dRSound.Id, soundData);
            }
        }

        public SoundData GetSoundDataBySoundId(int soundId)
        {
            if (dicSoundData.ContainsKey(soundId))
            {
                return dicSoundData[soundId];
            }

            return null;
        }

        public SoundGroupData GetSoundGroupDataBySoundId(int soundId)
        {
            if (dicSoundData.ContainsKey(soundId))
            {
                return dicSoundData[soundId].SoundGroupData;
            }

            return null;
        }

        public SoundPlayParamData GetSoundPlayParamDataBySoundId(int soundId)
        {
            if (dicSoundData.ContainsKey(soundId))
            {
                return dicSoundData[soundId].SoundPlayParam;
            }

            return null;
        }

        public SoundGroupData GetSoundGroupDataById(int id)
        {
            if (dicSoundGroupData.ContainsKey(id))
            {
                return dicSoundGroupData[id];
            }

            return null;
        }

        public SoundPlayParamData GetSoundPlayParamDataById(int id)
        {
            if (dicSoundPlayParamData.ContainsKey(id))
            {
                return dicSoundPlayParamData[id];
            }

            return null;
        }

        public SoundData[] GetAllSoundData()
        {
            int index = 0;
            SoundData[] results = new SoundData[dicSoundData.Count];
            foreach (var soundData in dicSoundData.Values)
            {
                results[index++] = soundData;
            }

            return results;
        }

        public SoundGroupData[] GetAllSoundGroupData()
        {
            int index = 0;
            SoundGroupData[] results = new SoundGroupData[dicSoundGroupData.Count];
            foreach (var soundGroupData in dicSoundGroupData.Values)
            {
                results[index++] = soundGroupData;
            }

            return results;
        }

        public SoundPlayParamData[] GetAllSoundPlayParamData()
        {
            int index = 0;
            SoundPlayParamData[] results = new SoundPlayParamData[dicSoundPlayParamData.Count];
            foreach (var soundPlayParamData in dicSoundPlayParamData.Values)
            {
                results[index++] = soundPlayParamData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRSound>();
            GameEntry.DataTable.DestroyDataTable<DRSoundGroup>();
            GameEntry.DataTable.DestroyDataTable<DRSoundPlayParam>();

            dtSound = null;
            dtSoundGroup = null;
            dtSoundPlayParam = null;
            dicSoundData = null;
            dicSoundGroupData = null;
            dicSoundPlayParamData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}