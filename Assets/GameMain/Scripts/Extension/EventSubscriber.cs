using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;
using System.Collections.Generic;

namespace Flower
{
    public class EventSubscriber
    {
        private GameFrameworkMultiDictionary<int, EventHandler<GameEventArgs>> dicEventHandler = new GameFrameworkMultiDictionary<int, EventHandler<GameEventArgs>>();
        private object owner;

        public object Owner
        {
            get
            {
                return owner;
            }
        }

        public EventSubscriber(object owner)
        {
            this.owner = owner;
            dicEventHandler = new GameFrameworkMultiDictionary<int, EventHandler<GameEventArgs>>();
        }

        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            dicEventHandler.Add(id, handler);
            GameEntry.Event.Subscribe(id, handler);
        }

        public void UnSubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (!dicEventHandler.Remove(id, handler))
            {
                throw new Exception(Utility.Text.Format("Event '{0}' not exists specified handler.", id.ToString()));
            }

            GameEntry.Event.Unsubscribe(id, handler);
        }

        public void UnSubscribeAll()
        {
            if (dicEventHandler == null)
                return;

            foreach (var item in dicEventHandler)
            {
                foreach (var eventHandler in item.Value)
                {
                    GameEntry.Event.Unsubscribe(item.Key, eventHandler);
                }
            }

            dicEventHandler.Clear();
        }
    }
}