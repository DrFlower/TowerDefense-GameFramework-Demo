using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class SellTowerEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SellTowerEventArgs).GetHashCode();

        public int TowerSerialId
        {
            get;
            private set;
        }

        public SellTowerEventArgs()
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

        public static SellTowerEventArgs Create(int towerSerialId, object userData = null)
        {
            SellTowerEventArgs sellTowerEventArgs = ReferencePool.Acquire<SellTowerEventArgs>();
            sellTowerEventArgs.TowerSerialId = towerSerialId;
            return sellTowerEventArgs;
        }

        public override void Clear()
        {
            TowerSerialId = 0;
        }
    }

}

