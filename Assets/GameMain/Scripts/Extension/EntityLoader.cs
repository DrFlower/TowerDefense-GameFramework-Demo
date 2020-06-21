using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;
using System.Collections.Generic;

namespace Flower
{
    public class EntityLoader : IReference
    {
        private Dictionary<int, Action<Entity>> dicCallback;
        private Dictionary<int, Entity> dicSerial2Entity;

        public object Owner
        {
            get;
            private set;
        }

        public EntityLoader()
        {
            dicSerial2Entity = new Dictionary<int, Entity>();
            dicCallback = new Dictionary<int, Action<Entity>>();
            Owner = null;
        }

        public int ShowEntity(EnumEntity enumEntity, Type entityLogicType, Action<Entity> onShowSuccess, object userData = null)
        {
            int serialId = GameEntry.Entity.GenerateSerialId();
            dicCallback.Add(serialId, onShowSuccess);
            GameEntry.Entity.ShowEntity(serialId, enumEntity, entityLogicType, userData);
            return serialId;
        }

        public int ShowEntity<T>(EnumEntity enumEntity, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            int serialId = GameEntry.Entity.GenerateSerialId();
            dicCallback.Add(serialId, onShowSuccess);
            GameEntry.Entity.ShowEntity<T>(serialId, enumEntity, userData);
            return serialId;
        }
        public bool HasEntity(int serialId)
        {
            return GetEntity(serialId) != null;
        }

        public Entity GetEntity(int serialId)
        {
            if (dicSerial2Entity.ContainsKey(serialId))
            {
                return dicSerial2Entity[serialId];
            }

            return null;
        }

        public void HideEntity(int serialId)
        {
            Entity entity = null;
            if (!dicSerial2Entity.TryGetValue(serialId, out entity))
            {
                Log.Error("Can find entity('serial id:{0}') ", serialId);
            }

            dicSerial2Entity.Remove(serialId);
            dicCallback.Remove(serialId);

            GameEntry.Entity.HideEntity(entity);
        }

        public void HideEntity(Entity entity)
        {
            if (entity == null)
                return;

            GameEntry.Entity.HideEntity(entity.Id);
        }

        public void HideAllEntity()
        {
            foreach (var serialId in dicSerial2Entity.Keys)
            {
                GameEntry.Entity.HideEntity(serialId);
            }

            dicSerial2Entity.Clear();
            dicCallback.Clear();
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne == null)
            {
                return;
            }

            Action<Entity> callback = null;
            if (!dicCallback.TryGetValue(ne.Entity.Id, out callback))
            {
                return;
            }

            dicSerial2Entity.Add(ne.Entity.Id, ne.Entity);
            callback(ne.Entity);
        }

        private void OnShowEntityFail(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            if (ne == null)
            {
                return;
            }

            if (dicCallback.ContainsKey(ne.EntityId))
            {
                dicCallback.Remove(ne.EntityId);
                Log.Warning("{0} Show entity failure with error message '{1}'.", Owner.ToString(), ne.ErrorMessage);
            }
        }

        public static EntityLoader Create(object owner)
        {
            EntityLoader entityLoader = ReferencePool.Acquire<EntityLoader>();
            entityLoader.Owner = owner;
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, entityLoader.OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, entityLoader.OnShowEntityFail);

            return entityLoader;
        }

        public void Clear()
        {
            Owner = null;
            dicSerial2Entity.Clear();
            dicCallback.Clear();
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFail);
        }
    }
}

