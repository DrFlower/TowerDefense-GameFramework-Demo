using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class UpgradeTowerEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UpgradeTowerEventArgs).GetHashCode();

        public int LastLevel
        {
            get;
            private set;
        }

        public Tower Tower
        {
            get;
            private set;
        }

        public UpgradeTowerEventArgs()
        {
            Tower = null;
            LastLevel = 0;
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

        public static UpgradeTowerEventArgs Create(Tower tower, int lastLevel, object userData = null)
        {
            UpgradeTowerEventArgs upgradeTowerEventArgs = ReferencePool.Acquire<UpgradeTowerEventArgs>();
            upgradeTowerEventArgs.Tower = tower;
            upgradeTowerEventArgs.LastLevel = lastLevel;
            return upgradeTowerEventArgs;
        }

        public override void Clear()
        {
            Tower = null;
            LastLevel = 0;
        }
    }

}

