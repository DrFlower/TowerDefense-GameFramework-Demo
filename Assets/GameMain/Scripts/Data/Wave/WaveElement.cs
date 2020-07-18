using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace Flower.Data
{
    public class WaveElement : IReference
    {
        private WaveElementData waveElementData;

        public int Id
        {
            get
            {
                return waveElementData.Id;
            }
        }

        public int EnemyId
        {
            get
            {
                return waveElementData.EnemyId;
            }
        }

        public float SpawnTime
        {
            get
            {
                return waveElementData.SpawnTime;
            }
        }

        public float CumulativeTime
        {
            get;
            private set;
        }

        public WaveElement()
        {
            waveElementData = null;
            CumulativeTime = 0;
        }

        public static WaveElement Create(WaveElementData waveElementData, float cumulativeTime)
        {
            WaveElement waveElement = ReferencePool.Acquire<WaveElement>();
            waveElement.waveElementData = waveElementData;
            waveElement.CumulativeTime = cumulativeTime;
            return waveElement;
        }

        public void Clear()
        {
            waveElementData = null;
            CumulativeTime = 0;
        }
    }
}
