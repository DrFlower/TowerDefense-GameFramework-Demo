using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace Flower
{
    public class PlayerHPChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerHPChangeEventArgs).GetHashCode();

        public int LastHP
        {
            get;
            private set;
        }

        public int CurrentHP
        {
            get;
            private set;
        }

        public PlayerHPChangeEventArgs()
        {
            LastHP = 0;
            CurrentHP = 0;
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

        public static PlayerHPChangeEventArgs Create(int lastHP, int currentHP, object userData = null)
        {
            PlayerHPChangeEventArgs playerHPChangeEventArgs = ReferencePool.Acquire<PlayerHPChangeEventArgs>();
            playerHPChangeEventArgs.LastHP = lastHP;
            playerHPChangeEventArgs.CurrentHP = currentHP;
            return playerHPChangeEventArgs;
        }

        public override void Clear()
        {
            LastHP = 0;
            CurrentHP = 0;
        }
    }

}

