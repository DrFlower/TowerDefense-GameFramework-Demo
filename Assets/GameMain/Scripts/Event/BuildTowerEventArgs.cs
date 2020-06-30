using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class BuildTowerEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(BuildTowerEventArgs).GetHashCode();

        public Vector3 Position
        {
            get;
            private set;
        }

        public Quaternion Rotation
        {
            get;
            private set;
        }

        public TowerData TowerData
        {
            get;
            private set;
        }

        public BuildTowerEventArgs()
        {
            TowerData = null;
            Position = Vector3.zero;
            Rotation = Quaternion.identity;
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

        public static BuildTowerEventArgs Create(TowerData towerData, Vector3 position, Quaternion rotation, object userData = null)
        {
            BuildTowerEventArgs buildTowerEventArgs = ReferencePool.Acquire<BuildTowerEventArgs>();
            buildTowerEventArgs.TowerData = towerData;
            buildTowerEventArgs.Position = position;
            buildTowerEventArgs.Rotation = rotation;
            return buildTowerEventArgs;
        }

        public override void Clear()
        {
            TowerData = null;
            Position = Vector3.zero;
            Rotation = Quaternion.identity;
        }
    }

}

