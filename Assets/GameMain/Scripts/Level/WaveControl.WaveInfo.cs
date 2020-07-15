using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;
using GameFramework;

namespace Flower
{
    partial class WaveControl : IReference
    {
        private class WaveInfo : IReference
        {
            private Queue<WaveElementInfo> waveElementInfos;

            private bool finished = false;

            public float TotalSpawnTime
            {
                get;
                private set;
            }

            public float NextWaveTime
            {
                get;
                private set;
            }

            public int CurrentWave
            {
                get;
                private set;
            }

            public int TotalWave
            {
                get;
                private set;
            }

            public float TotalTime
            {
                get
                {
                    return TotalSpawnTime + NextWaveTime;
                }
            }

            public WaveInfo()
            {
                waveElementInfos = new Queue<WaveElementInfo>();
                CurrentWave = 1;
                TotalSpawnTime = 0f;
                NextWaveTime = 0f;
                finished = false;
            }

            public int DequeueEnemy(float time)
            {
                if (waveElementInfos.Count > 0)
                {
                    if (time > waveElementInfos.Peek().Time)
                    {
                        WaveElementInfo waveElementInfo = waveElementInfos.Dequeue();
                        int enemyId = waveElementInfo.EnemyId;
                        ReferencePool.Release(waveElementInfo);
                        waveElementInfo = null;
                        CurrentWave++;
                        return enemyId;
                    }
                }
                else if (time >= TotalTime)
                {
                    return -1;
                }
                return 0;
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

                waveInfo.TotalWave = waveData.WaveElementDatas.Length;
                waveInfo.TotalSpawnTime = timer;
                waveInfo.NextWaveTime = timer + waveData.FinishWaitTIme;
                return waveInfo;
            }

            public void Clear()
            {
                while (waveElementInfos.Count > 0)
                {
                    WaveElementInfo waveElementInfo = waveElementInfos.Dequeue();
                    ReferencePool.Release(waveElementInfo);
                }

                waveElementInfos.Clear();
                CurrentWave = 1;
                TotalWave = 0;
                TotalSpawnTime = 0f;
                NextWaveTime = 0f;
                finished = false;
            }
        }
    }
}