using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class HidePreviewTowerEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HidePreviewTowerEventArgs).GetHashCode();

        public TowerData TowerData
        {
            get;
            set;
        }

        public HidePreviewTowerEventArgs()
        {
            TowerData = null;
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

        public static HidePreviewTowerEventArgs Create(TowerData towerData, object userData = null)
        {
            HidePreviewTowerEventArgs hidePreviewTowerEventArgs = ReferencePool.Acquire<HidePreviewTowerEventArgs>();
            hidePreviewTowerEventArgs.TowerData = towerData;
            return hidePreviewTowerEventArgs;
        }

        public override void Clear()
        {
            TowerData = null;
        }
    }

}

