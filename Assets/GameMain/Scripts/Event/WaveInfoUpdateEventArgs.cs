using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class WaveInfoUpdateEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WaveInfoUpdateEventArgs).GetHashCode();

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

        public float CurrentWaveProgress
        {
            get;
            private set;
        }

        public WaveInfoUpdateEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }


        public object UserData
        {
            get;
            private set;
        }

        public static WaveInfoUpdateEventArgs Create(int currentWave, int totalWave, float currentWaveProgress, object userData = null)
        {
            WaveInfoUpdateEventArgs waveInfoUpdateEventArgs = ReferencePool.Acquire<WaveInfoUpdateEventArgs>();
            waveInfoUpdateEventArgs.CurrentWave = currentWave;
            waveInfoUpdateEventArgs.TotalWave = totalWave;
            waveInfoUpdateEventArgs.CurrentWaveProgress = currentWaveProgress;
            return waveInfoUpdateEventArgs;
        }

        public override void Clear()
        {
            CurrentWave = 0;
            TotalWave = 0;
            CurrentWaveProgress = 0;
        }
    }

}

