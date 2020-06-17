using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace Flower
{
    public class LoadLevelFinishEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadLevelFinishEventArgs).GetHashCode();

        public LoadLevelFinishEventArgs()
        {
            LevelId = -1;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int LevelId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static LoadLevelFinishEventArgs Create(int levelId, object userData = null)
        {
            LoadLevelFinishEventArgs loadLevelEventArgs = ReferencePool.Acquire<LoadLevelFinishEventArgs>();
            loadLevelEventArgs.LevelId = levelId;
            loadLevelEventArgs.UserData = userData;
            return loadLevelEventArgs;
        }

        public override void Clear()
        {
            LevelId = -1;
        }
    }

}

