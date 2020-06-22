using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class LoadLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadLevelEventArgs).GetHashCode();

        public LoadLevelEventArgs()
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

        public static LoadLevelEventArgs Create(LevelData levelData, object userData = null)
        {
            LoadLevelEventArgs loadLevelEventArgs = ReferencePool.Acquire<LoadLevelEventArgs>();
            loadLevelEventArgs.LevelData = levelData;
            loadLevelEventArgs.UserData = userData;
            return loadLevelEventArgs;
        }

        public override void Clear()
        {
            LevelData = null;
        }
    }

}

