using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using GameFramework.Event;
using GameFramework;

namespace Flower
{
    public abstract class ItemLogicEx : ItemLogic
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

        public int ShowItem(EnumItem enumItem, Action<Item> onShowSuccess, object userData = null)
        {
            if (itemLoader == null)
            {
                itemLoader = ItemLoader.Create(this);
            }

            return itemLoader.ShowItem(enumItem, onShowSuccess, userData);
        }

        public int ShowItem(int itemId, Action<Item> onShowSuccess, object userData = null)
        {
            if (itemLoader == null)
            {
                itemLoader = ItemLoader.Create(this);
            }

            return itemLoader.ShowItem(itemId, onShowSuccess, userData);
        }

        public int ShowItem<T>(EnumItem enumItem, Action<Item> onShowSuccess, object userData = null) where T : ItemLogic
        {
            if (itemLoader == null)
            {
                itemLoader = ItemLoader.Create(this);
            }

            return itemLoader.ShowItem<T>(enumItem, onShowSuccess, userData);
        }

        public int ShowItem<T>(int itemId, Action<Item> onShowSuccess, object userData = null) where T : ItemLogic
        {
            if (itemLoader == null)
            {
                itemLoader = ItemLoader.Create(this);
            }

            return itemLoader.ShowItem<T>(itemId, onShowSuccess, userData);
        }


        public bool HasItem(int serialId)
        {
            if (itemLoader == null)
            {
                return false;
            }

            return itemLoader.HasItem(serialId);
        }

        public Item GetItem(int serialId)
        {
            if (itemLoader == null)
            {
                return null;
            }

            return itemLoader.GetItem(serialId);
        }

        public void HideItem(int serialId)
        {
            if (itemLoader == null)
            {
                return;
            }

            itemLoader.HideItem(serialId);
        }

        public void HideItem(Item item)
        {
            if (itemLoader == null)
            {
                return;
            }

            itemLoader.HideItem(item);
        }

        public void HideAllItem()
        {
            if (itemLoader == null)
            {
                return;
            }

            itemLoader.HideAllItem();
        }

        public int ShowEntity(EnumEntity enumEntity, Type entityLogicType, Action<Entity> onShowSuccess, object userData = null)
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity(enumEntity, entityLogicType, onShowSuccess, userData);
        }

        public int ShowEntity(int entityId, Type entityLogicType, Action<Entity> onShowSuccess, object userData = null)
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity(entityId, entityLogicType, onShowSuccess, userData);
        }

        public int ShowItem<T>(EnumEntity enumEntity, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity<T>(enumEntity, onShowSuccess, userData);
        }

        public int ShowItem<T>(int entityId, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity<T>(entityId, onShowSuccess, userData);
        }

        public bool HasEntity(int serialId)
        {
            if (entityLoader == null)
                return false;

            return entityLoader.GetEntity(serialId);
        }

        public Entity GetEntity(int serialId)
        {
            if (entityLoader == null)
                return null;

            return entityLoader.GetEntity(serialId);
        }

        public void HideEntity(int serialId)
        {
            if (entityLoader == null)
            {
                return;
            }

            entityLoader.HideEntity(serialId);
        }

        public void HideEntity(Entity entity)
        {
            if (entityLoader == null)
            {
                return;
            }

            entityLoader.HideEntity(entity);
        }

        public void HideAllEntity()
        {
            if (entityLoader == null)
            {
                return;
            }

            entityLoader.HideAllEntity();
        }
    }
}
