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
            private float totalSpawnTime = 0f;
            private float nextWaveTime = 0f;
            private bool finished = false;

            public WaveInfo()
            {
                waveElementInfos = new Queue<WaveElementInfo>();
                totalSpawnTime = 0f;
                nextWaveTime = 0f;
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
                        return enemyId;
                    }
                }

                if (time > nextWaveTime)
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

                waveInfo.totalSpawnTime = timer;
                waveInfo.nextWaveTime = timer + waveData.FinishWaitTIme;
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
                totalSpawnTime = 0f;
                nextWaveTime = 0f;
                finished = false;
            }
        }
    }
}