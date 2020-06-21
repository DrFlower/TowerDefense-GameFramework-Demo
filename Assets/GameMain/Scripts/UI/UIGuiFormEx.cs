using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using GameFramework.Event;
using GameFramework;

namespace Flower
{
    public class UGuiFormEx : UGuiForm
    {
        private EventSubscriber eventSubscriber;
        private ItemLoader itemLoader;

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            UnSubscribeAll();
            if (eventSubscriber != null)
            {
                ReferencePool.Release(eventSubscriber);
                eventSubscriber = null;
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
        public int ShowItem<T>(EnumItem enumItem, Action<Item> onShowSuccess, object userData = null) where T : ItemLogic
        {
            if (itemLoader == null)
            {
                itemLoader = ItemLoader.Create(this);
            }

            return itemLoader.ShowItem<T>(enumItem, onShowSuccess, userData);
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
    }

}
