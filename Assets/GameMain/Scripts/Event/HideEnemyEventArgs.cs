using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class HideEnemyEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HideEnemyEventArgs).GetHashCode();

        public int EntityId
        {
            get;
            private set;
        }

        public HideEnemyEventArgs()
        {
            EntityId = -1;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static HideEnemyEventArgs Create(int entityId, object userData = null)
        {
            HideEnemyEventArgs HideEnemyEventArgs = ReferencePool.Acquire<HideEnemyEventArgs>();
            HideEnemyEventArgs.EntityId = entityId;
            return HideEnemyEventArgs;
        }

        public override void Clear()
        {
            EntityId = -1;
        }
    }

}

