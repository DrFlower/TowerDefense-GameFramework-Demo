using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class ReloadLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ReloadLevelEventArgs).GetHashCode();

        public ReloadLevelEventArgs()
        {
            LevelData = null;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public LevelData LevelData
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ReloadLevelEventArgs Create(LevelData levelData, object userData = null)
        {
            ReloadLevelEventArgs reloadLevelEventArgs = ReferencePool.Acquire<ReloadLevelEventArgs>();
            reloadLevelEventArgs.LevelData = levelData;
            reloadLevelEventArgs.UserData = userData;
            return reloadLevelEventArgs;
        }

        public override void Clear()
        {
            LevelData = null;
        }
    }

}

