using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;
using System.Collections.Generic;

namespace Flower
{
    public class ItemLoader : IReference
    {
        private Dictionary<int, Action<Item>> dicCallback;
        private Dictionary<int, Item> dicSerial2Item;

        private List<int> tempList;

        public object Owner
        {
            get;
            private set;
        }

        public ItemLoader()
        {
            dicSerial2Item = new Dictionary<int, Item>();
            dicCallback = new Dictionary<int, Action<Item>>();
            tempList = new List<int>();
            Owner = null;
        }

        public int ShowItem(EnumItem enumItem, Action<Item> onShowSuccess, object userData = null)
        {
            return ShowItem((int)enumItem, onShowSuccess, userData);
        }

        public int ShowItem(int itemId, Action<Item> onShowSuccess, object userData = null)
        {
            int serialId = GameEntry.Item.GenerateSerialId();
            dicCallback.Add(serialId, onShowSuccess);
            GameEntry.Item.ShowItem(serialId, itemId, userData);
            return serialId;
        }

        public int ShowItem<T>(EnumItem enumItem, Action<Item> onShowSuccess, object userData = null) where T : ItemLogic
        {
            return ShowItem<T>((int)enumItem, onShowSuccess, userData);
        }

        public int ShowItem<T>(int itemId, Action<Item> onShowSuccess, object userData = null) where T : ItemLogic
        {
            int serialId = GameEntry.Item.GenerateSerialId();
            dicCallback.Add(serialId, onShowSuccess);
            GameEntry.Item.ShowItem<T>(serialId, itemId, userData);
            return serialId;
        }

        public bool HasItem(int serialId)
        {
            return GetItem(serialId) != null;
        }

        public Item GetItem(int serialId)
        {
            if (dicSerial2Item.ContainsKey(serialId))
            {
                return dicSerial2Item[serialId];
            }

            return null;
        }

        public void HideItem(int serialId)
        {
            Item item = null;
            if (!dicSerial2Item.TryGetValue(serialId, out item))
            {
                Log.Error("Can find item('serial id:{0}') ", serialId);
            }

            dicSerial2Item.Remove(serialId);
            dicCallback.Remove(serialId);

            GameEntry.Item.HideItem(item);
        }

        public void HideItem(Item item)
        {
            if (item == null)
                return;

            HideItem(item.Id);
        }

        public void HideAllItem()
        {
            tempList.Clear();

            foreach (var serialId in dicSerial2Item.Keys)
            {
                tempList.Add(serialId);
            }

            foreach (var serialId in tempList)
            {
                HideItem(serialId);
            }

            dicSerial2Item.Clear();
            dicCallback.Clear();
        }

        private void OnShowItemSuccess(object sender, GameEventArgs e)
        {
            ShowItemSuccessEventArgs ne = (ShowItemSuccessEventArgs)e;
            if (ne == null)
            {
                return;
            }

            Action<Item> callback = null;
            if (!dicCallback.TryGetValue(ne.Item.Id, out callback))
            {
                return;
            }

            dicSerial2Item.Add(ne.Item.Id, ne.Item);

            callback?.Invoke(ne.Item);
        }

        private void OnShowItemFail(object sender, GameEventArgs e)
        {
            ShowItemFailureEventArgs ne = (ShowItemFailureEventArgs)e;
            if (ne == null)
            {
                return;
            }

            if (dicCallback.ContainsKey(ne.ItemId))
            {
                dicCallback.Remove(ne.ItemId);
                Log.Warning("{0} Show item failure with error message '{1}'.", Owner.ToString(), ne.ErrorMessage);
            }
        }

        public static ItemLoader Create(object owner)
        {
            ItemLoader itemLoader = ReferencePool.Acquire<ItemLoader>();
            itemLoader.Owner = owner;
            GameEntry.Event.Subscribe(ShowItemSuccessEventArgs.EventId, itemLoader.OnShowItemSuccess);
            GameEntry.Event.Subscribe(ShowItemFailureEventArgs.EventId, itemLoader.OnShowItemFail);

            return itemLoader;
        }

        public void Clear()
        {
            Owner = null;
            dicSerial2Item.Clear();
            dicCallback.Clear();
            GameEntry.Event.Unsubscribe(ShowItemSuccessEventArgs.EventId, OnShowItemSuccess);
            GameEntry.Event.Unsubscribe(ShowItemFailureEventArgs.EventId, OnShowItemFail);
        }
    }
}

