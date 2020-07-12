using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using GameFramework.Event;
using GameFramework;

namespace Flower
{
    public abstract class EntityLogicEx : EntityLogicWithData
    {
        private EventSubscriber eventSubscriber;
        private EntityLoader entityLoader;
        private ItemLoader itemLoader;

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            UnSubscribeAll();
            if (eventSubscriber != null)
            {
                ReferencePool.Release(eventSubscriber);
                eventSubscriber = null;
            }

            HideAllEntity();
            if (entityLoader != null)
            {
                ReferencePool.Release(entityLoader);
                entityLoader = null;
            }

            HideAllItem();
            if (itemLoader != null)
            {
                ReferencePool.Release(itemLoader);
                itemLoader = null;
            }
        }
        protected void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (eventSubscriber == null)
                eventSubscriber = EventSubscriber.Create(this);

            eventSubscriber.Subscribe(id, handler);
        }

        protected void UnSubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (eventSubscriber != null)
                eventSubscriber.UnSubscribe(id, handler);
        }

        protected void UnSubscribeAll()
        {
            if (eventSubscriber != null)
                eventSubscriber.UnSubscribeAll();
        }

        protected int ShowItem(EnumItem enumItem, Action<Item> onShowSuccess, object userData = null)
        {
            if (itemLoader == null)
            {
                itemLoader = ItemLoader.Create(this);
            }

            return itemLoader.ShowItem(enumItem, onShowSuccess, userData);
        }

        protected int ShowItem(int itemId, Action<Item> onShowSuccess, object userData = null)
        {
            if (itemLoader == null)
            {
                itemLoader = ItemLoader.Create(this);
            }

            return itemLoader.ShowItem(itemId, onShowSuccess, userData);
        }

        protected int ShowItem<T>(EnumItem enumItem, Action<Item> onShowSuccess, object userData = null) where T : ItemLogic
        {
            if (itemLoader == null)
            {
                itemLoader = ItemLoader.Create(this);
            }

            return itemLoader.ShowItem<T>(enumItem, onShowSuccess, userData);
        }

        protected int ShowItem<T>(int itemId, Action<Item> onShowSuccess, object userData = null) where T : ItemLogic
        {
            if (itemLoader == null)
            {
                itemLoader = ItemLoader.Create(this);
            }

            return itemLoader.ShowItem<T>(itemId, onShowSuccess, userData);
        }


        protected bool HasItem(int serialId)
        {
            if (itemLoader == null)
            {
                return false;
            }

            return itemLoader.HasItem(serialId);
        }

        protected Item GetItem(int serialId)
        {
            if (itemLoader == null)
            {
                return null;
            }

            return itemLoader.GetItem(serialId);
        }

        protected void HideItem(int serialId)
        {
            if (itemLoader == null)
            {
                return;
            }

            itemLoader.HideItem(serialId);
        }

        protected void HideItem(Item item)
        {
            if (itemLoader == null)
            {
                return;
            }

            itemLoader.HideItem(item);
        }

        protected void HideAllItem()
        {
            if (itemLoader == null)
            {
                return;
            }

            itemLoader.HideAllItem();
        }

        protected int ShowEntity(EnumEntity enumEntity, Type entityLogicType, Action<Entity> onShowSuccess, object userData = null)
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity(enumEntity, entityLogicType, onShowSuccess, userData);
        }

        protected int ShowEntity(int entityId, Type entityLogicType, Action<Entity> onShowSuccess, object userData = null)
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity(entityId, entityLogicType, onShowSuccess, userData);
        }

        protected int ShowEntity<T>(EnumEntity enumEntity, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity<T>(enumEntity, onShowSuccess, userData);
        }

        protected int ShowEntity<T>(int entityId, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity<T>(entityId, onShowSuccess, userData);
        }

        protected bool HasEntity(int serialId)
        {
            if (entityLoader == null)
                return false;

            return entityLoader.GetEntity(serialId);
        }

        protected Entity GetEntity(int serialId)
        {
            if (entityLoader == null)
                return null;

            return entityLoader.GetEntity(serialId);
        }

        protected void HideEntity(int serialId)
        {
            if (entityLoader == null)
            {
                return;
            }

            entityLoader.HideEntity(serialId);
        }

        protected void HideEntity(Entity entity)
        {
            if (entityLoader == null)
            {
                return;
            }

            entityLoader.HideEntity(entity);
        }

        protected void HideAllEntity()
        {
            if (entityLoader == null)
            {
                return;
            }

            entityLoader.HideAllEntity();
        }
    }
}
