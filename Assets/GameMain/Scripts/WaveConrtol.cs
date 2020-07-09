using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;
using GameFramework;

namespace Flower
{
    class WaveControl : IReference
    {
        private class WaveElementInfo : IReference
        {
            public WaveElementInfo()
            {
                EnemyId = 0;
                Time = 0;
            }

            public int EnemyId
            {
                get;
                private set;
            }

            public float Time
            {
                get;
                private set;
            }

            public static WaveElementInfo Create(int enemyId, float time)
            {
                WaveElementInfo waveElementInfo = ReferencePool.Acquire<WaveElementInfo>();
                waveElementInfo.EnemyId = enemyId;
                waveElementInfo.Time = time;
                return waveElementInfo;
            }

            public void Clear()
            {
                EnemyId = 0;
                Time = 0;
            }
        }

        private class WaveInfo : IReference
        {
            private Queue<WaveElementInfo> waveElementInfos;

            public WaveInfo()
            {
                waveElementInfos = new Queue<WaveElementInfo>();
            }

            public int GetEnemyByTime(float time)
            {
                if (waveElementInfos.Count > 0)
                {
                    if (waveElementInfos.Peek().Time > time)
                    {
                        WaveElementInfo waveElementInfo = waveElementInfos.Dequeue();
                        int enemyId = waveElementInfo.EnemyId;
                        ReferencePool.Release(waveElementInfo);
                        return enemyId;
                    }
                }

                return -1;
            }

            public static WaveInfo Create(WaveData waveData)
            {
                WaveInfo waveInfo = ReferencePool.Acquire<WaveInfo>();
                WaveElementData[] waveElementDatas = waveData.WaveElementDatas;
                float timer = 0;

                foreach (var waveElementData in waveElementDatas)
                {
                    timer += waveElementData.SpawnTime;
                    waveInfo.waveElementInfos.Enqueue(WaveElementInfo.Create(waveElementData.EnemyId, timer));
                }

                return waveInfo;
            }

            public void Clear()
            {
                waveElementInfos.Clear();
            }
        }

        private Queue<WaveData> waveDatas;
        private DataLevel dataLevel;

        private float timer = 0;

        public WaveControl()
        {
            waveDatas = new Queue<WaveData>();
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (dataLevel.LevelState == EnumLevelState.Normal)
            {
                timer += elapseSeconds;

                if (waveDatas.Count > 0)
                {
                    WaveData waveData = waveDatas.Dequeue();

                }

            }
        }

        public void StartWave()
        {

        }

        public void OnPause()
        {

        }

        public void OnResume()
        {

        }

        public void OnRestart()
        {

        }

        public void OnGameover()
        {

        }

        public void OnQuick()
        {

        }

        public static WaveControl Create(WaveData[] waveDatas)
        {
            WaveControl waveControl = ReferencePool.Acquire<WaveControl>();
            for (int i = 0; i < waveDatas.Length; i++)
            {
                waveControl.waveDatas.Enqueue(waveDatas[i]);
            }

            waveControl.dataLevel = GameEntry.Data.GetData<DataLevel>();

            return waveControl;
        }

        public void Clear()
        {
            waveDatas.Clear();
            dataLevel = null;
        }
    }
}
