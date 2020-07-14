using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace Flower
{
    public class GameoverEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GameoverEventArgs).GetHashCode();

        public GameoverEventArgs()
        {
            EnumGameOverType = EnumGameOverType.Fail;
            StarCount = 0;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public EnumGameOverType EnumGameOverType
        {
            get;
            private set;
        }

        public int StarCount
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static GameoverEventArgs Create(EnumGameOverType enumGameOverType, int starCount, object userData = null)
        {
            GameoverEventArgs gameoverEventArgs = ReferencePool.Acquire<GameoverEventArgs>();
            gameoverEventArgs.EnumGameOverType = enumGameOverType;
            gameoverEventArgs.StarCount = starCount;
            return gameoverEventArgs;
        }

        public override void Clear()
        {
            EnumGameOverType = EnumGameOverType.Fail;
            StarCount = 0;
        }
    }

}

