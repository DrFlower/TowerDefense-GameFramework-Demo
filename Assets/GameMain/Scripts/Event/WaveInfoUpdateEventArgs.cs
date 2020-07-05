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

        public int LastWave
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

        public static WaveInfoUpdateEventArgs Create(int lastWave, int currentWave, int totalWave, object userData = null)
        {
            WaveInfoUpdateEventArgs waveInfoUpdateEventArgs = ReferencePool.Acquire<WaveInfoUpdateEventArgs>();
            waveInfoUpdateEventArgs.LastWave = lastWave;
            waveInfoUpdateEventArgs.CurrentWave = currentWave;
            waveInfoUpdateEventArgs.TotalWave = totalWave;
            return waveInfoUpdateEventArgs;
        }

        public override void Clear()
        {
            LastWave = 0;
            CurrentWave = 0;
            TotalWave = 0;
        }
    }

}

