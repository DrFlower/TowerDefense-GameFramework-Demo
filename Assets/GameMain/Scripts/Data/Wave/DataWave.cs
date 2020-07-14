using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace Flower.Data
{
    public sealed class DataWave : DataBase
    {
        private IDataTable<DRWave> dtWave;
        private IDataTable<DRWaveElement> dtWaveElement;

        private Dictionary<int, WaveData> dicWaveData;
        private Dictionary<int, WaveElementData> dicWaveElementData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Wave");
            LoadDataTable("WaveElement");
        }

        protected override void OnLoad()
        {
            dtWave = GameEntry.DataTable.GetDataTable<DRWave>();
            if (dtWave == null)
                throw new System.Exception("Can not get data table Item");

            dtWaveElement = GameEntry.DataTable.GetDataTable<DRWaveElement>();
            if (dtWaveElement == null)
                throw new System.Exception("Can not get data table ItemGroup");

            dicWaveData = new Dictionary<int, WaveData>();
            dicWaveElementData = new Dictionary<int, WaveElementData>();

            DRWaveElement[] dRWaveElements = dtWaveElement.GetAllDataRows();
            foreach (var dRWaveElement in dRWaveElements)
            {
                if (dicWaveElementData.ContainsKey(dRWaveElement.Id))
                {
                    Log.Error("WaveElement id duplicate:{0}.", dRWaveElement.Id);
                    continue;
                }

                dicWaveElementData.Add(dRWaveElement.Id, new WaveElementData(dRWaveElement));
            }

            DRWave[] dRWaves = dtWave.GetAllDataRows();
            foreach (var dRWave in dRWaves)
            {
                int[] waveElementRange = dRWave.WaveElements;
                if (waveElementRange.Length != 2)
                    throw new System.Exception(string.Format("Wave data 'WaveElements' length error,current is '{0}', should be 2", waveElementRange.Length));

                int startIndex = waveElementRange[0];
                int endIndex = waveElementRange[1];

                if (endIndex < startIndex)
                    throw new System.Exception("Wave element index invaild,EndIndex should smaller than StartIndex.");

                WaveElementData[] waveElementDatas = new WaveElementData[endIndex - startIndex + 1];

                int index = 0;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    WaveElementData waveElementData = null;
                    if (!dicWaveElementData.TryGetValue(i, out waveElementData))
                    {
                        throw new System.Exception("Can not find WaveElementDat id :" + i);
                    }
                    waveElementDatas[index++] = waveElementData;
                }

                dicWaveData.Add(dRWave.Id, new WaveData(dRWave, waveElementDatas));
            }
        }

        public WaveData GetWaveData(int id)
        {
            if (dicWaveData.ContainsKey(id))
            {
                return dicWaveData[id];
            }

            return null;
        }

        public WaveData[] GetAllWaveData()
        {
            int index = 0;
            WaveData[] results = new WaveData[dicWaveData.Count];
            foreach (var waveData in dicWaveData.Values)
            {
                results[index++] = waveData;
            }

            return results;
        }

        public WaveElementData GetWaveElementData(int id)
        {
            if (dicWaveData.ContainsKey(id))
            {
                return dicWaveElementData[id];
            }

            return null;
        }

        public WaveElementData[] GetAllWaveElementData()
        {
            int index = 0;
            WaveElementData[] results = new WaveElementData[dicWaveElementData.Count];
            foreach (var waveElementData in dicWaveElementData.Values)
            {
                results[index++] = waveElementData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRWave>();
            GameEntry.DataTable.DestroyDataTable<DRWaveElement>();

            dtWave = null;
            dtWaveElement = null;
            dicWaveData = null;
            dicWaveElementData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}