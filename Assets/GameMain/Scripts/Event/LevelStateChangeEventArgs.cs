using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class LevelStateChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LevelStateChangeEventArgs).GetHashCode();

        public LevelData LevelData
        {
            get;
            private set;
        }

        public EnumLevelState LastState
        {
            get;
            private set;
        }

        public EnumLevelState CurrentState
        {
            get;
            private set;
        }

        public LevelStateChangeEventArgs()
        {
            LastState = EnumLevelState.None;
            CurrentState = EnumLevelState.None;
            LevelData = null;
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

        public static LevelStateChangeEventArgs Create(LevelData levelData, EnumLevelState lastState, EnumLevelState currentState, object userData = null)
        {
            LevelStateChangeEventArgs levelStateChangeEventArgs = ReferencePool.Acquire<LevelStateChangeEventArgs>();
            levelStateChangeEventArgs.LevelData = levelData;
            levelStateChangeEventArgs.LastState = lastState;
            levelStateChangeEventArgs.CurrentState = currentState;
            return levelStateChangeEventArgs;
        }

        public override void Clear()
        {
            LastState = EnumLevelState.None;
            CurrentState = EnumLevelState.None;
            LevelData = null;
        }
    }

}

