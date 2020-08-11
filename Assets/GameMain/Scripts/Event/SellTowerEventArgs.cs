using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class HideTowerInLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HideTowerInLevelEventArgs).GetHashCode();

        public int TowerSerialId
        {
            get;
            private set;
        }

        public HideTowerInLevelEventArgs()
        {
            TowerSerialId = 0;
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

        public static HideTowerInLevelEventArgs Create(int towerSerialId, object userData = null)
        {
            HideTowerInLevelEventArgs sellTowerEventArgs = ReferencePool.Acquire<HideTowerInLevelEventArgs>();
            sellTowerEventArgs.TowerSerialId = towerSerialId;
            return sellTowerEventArgs;
        }

        public override void Clear()
        {
            TowerSerialId = 0;
        }
    }

}

