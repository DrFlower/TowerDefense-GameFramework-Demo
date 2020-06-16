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

        public static GameoverEventArgs Create(object userData = null)
        {
            GameoverEventArgs gameoverEventArgs = ReferencePool.Acquire<GameoverEventArgs>();
            return gameoverEventArgs;
        }

        public override void Clear()
        {

        }
    }

}

