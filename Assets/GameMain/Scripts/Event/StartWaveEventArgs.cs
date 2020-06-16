using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace Flower
{
    public class StartWaveEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(StartWaveEventArgs).GetHashCode();

        public StartWaveEventArgs()
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

        public static StartWaveEventArgs Create(object userData = null)
        {
            StartWaveEventArgs startWaveEventArgs = ReferencePool.Acquire<StartWaveEventArgs>();
            return startWaveEventArgs;
        }

        public override void Clear()
        {

        }
    }

}

