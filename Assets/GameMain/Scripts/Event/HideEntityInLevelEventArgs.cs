using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class HideEntityInLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HideEntityInLevelEventArgs).GetHashCode();

        public int EntityId
        {
            get;
            private set;
        }

        public HideEntityInLevelEventArgs()
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

        public static HideEntityInLevelEventArgs Create(int entityId, object userData = null)
        {
            HideEntityInLevelEventArgs HideEntityInLevelEventArgs = ReferencePool.Acquire<HideEntityInLevelEventArgs>();
            HideEntityInLevelEventArgs.EntityId = entityId;
            return HideEntityInLevelEventArgs;
        }

        public override void Clear()
        {
            EntityId = -1;
        }
    }

}

