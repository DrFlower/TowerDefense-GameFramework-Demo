using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class ShowPreviewTowerEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowPreviewTowerEventArgs).GetHashCode();

        public TowerData TowerData
        {
            get;
            private set;
        }

        public ShowPreviewTowerEventArgs()
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

        public static ShowPreviewTowerEventArgs Create(TowerData towerData, object userData = null)
        {
            ShowPreviewTowerEventArgs showPreviewTowerEventArgs = ReferencePool.Acquire<ShowPreviewTowerEventArgs>();
            showPreviewTowerEventArgs.TowerData = towerData;
            return showPreviewTowerEventArgs;
        }

        public override void Clear()
        {
            TowerData = null;
        }
    }

}

