using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Flower.Data;
using System;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class ShowEntityInLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowEntityInLevelEventArgs).GetHashCode();

        public int EntityId
        {
            get;
            private set;
        }

        public Type Type
        {
            get;
            private set;
        }

        public Action<Entity> ShowSuccess
        {
            get;
            private set;
        }

        public EntityData EntityData
        {
            get;
            private set;
        }

        public ShowEntityInLevelEventArgs()
        {
            EntityId = -1;
            Type = null;
            ShowSuccess = null;
            EntityData = null;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static ShowEntityInLevelEventArgs Create(int entityId, Type entityType, Action<Entity> showSuccess, EntityData entityData, object userData = null)
        {
            ShowEntityInLevelEventArgs ShowEntityInLevelEventArgs = ReferencePool.Acquire<ShowEntityInLevelEventArgs>();
            ShowEntityInLevelEventArgs.EntityId = entityId;
            ShowEntityInLevelEventArgs.Type = entityType;
            ShowEntityInLevelEventArgs.ShowSuccess = showSuccess;
            ShowEntityInLevelEventArgs.EntityData = entityData;
            return ShowEntityInLevelEventArgs;
        }

        public override void Clear()
        {
            EntityId = -1;
            Type = null;
            ShowSuccess = null;
            EntityData = null;
        }
    }
}

