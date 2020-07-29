using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace Flower
{
    public class PlayerEnergyChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerEnergyChangeEventArgs).GetHashCode();

        public float LastEnergy
        {
            get;
            private set;
        }

        public float CurrentEnergy
        {
            get;
            private set;
        }

        public PlayerEnergyChangeEventArgs()
        {
            LastEnergy = 0;
            CurrentEnergy = 0;
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

        public static PlayerEnergyChangeEventArgs Create(float lastEnergy, float currentEnergy, object userData = null)
        {
            PlayerEnergyChangeEventArgs playerEnergyChangeEventArgs = ReferencePool.Acquire<PlayerEnergyChangeEventArgs>();
            playerEnergyChangeEventArgs.LastEnergy = lastEnergy;
            playerEnergyChangeEventArgs.CurrentEnergy = currentEnergy;
            return playerEnergyChangeEventArgs;
        }

        public override void Clear()
        {
            LastEnergy = 0;
            CurrentEnergy = 0;
        }
    }

}

