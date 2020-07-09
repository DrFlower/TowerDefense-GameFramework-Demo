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
    }
}