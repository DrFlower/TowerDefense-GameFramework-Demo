using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using Flower.Data;

namespace Flower
{
    public static class ItemExtension
    {
        private static int s_SerialId = 0;

        public static void ShowItem(this ItemComponent itemComponent, int serialId, EnumItem enumItem, object userData = null)
        {
            itemComponent.ShowItem(serialId, enumItem, null, userData);
        }

        public static void ShowItem<T>(this ItemComponent itemComponent, int serialId, EnumItem enumItem, object userData = null)
        {
            itemComponent.ShowItem(serialId, enumItem, typeof(T), userData);
        }

        public static void ShowItem(this ItemComponent itemComponent, int serialId, EnumItem enumItem, Type logicType, object userData = null)
        {
            itemComponent.ShowItem(serialId, (int)enumItem, logicType, userData);
        }

        public static void ShowItem(this ItemComponent itemComponent, int serialId, int itemId, object userData = null)
        {
            itemComponent.ShowItem(serialId, itemId, null, userData);
        }

        public static void ShowItem<T>(this ItemComponent itemComponent, int serialId, int itemId, object userData = null)
        {
            itemComponent.ShowItem(serialId, itemId, typeof(T), userData);
        }

        public static void ShowItem(this ItemComponent itemComponent, int serialId, int itemId, Type logicType, object userData = null)
        {
            ItemData itemData = GameEntry.Data.GetData<Data.DataItem>().GetItemData(itemId);

            if (itemData == null)
            {
                Log.Warning("Can not load item id '{0}' from data table.", itemData.Id.ToString());
                return;
            }

            itemComponent.ShowItem(serialId, logicType, itemData.AssetPath, itemData.ItemGroupData.Name, Constant.AssetPriority.ItemAsset, userData);
        }

        public static int GenerateSerialId(this ItemComponent itemComponent)
        {
            return ++s_SerialId;
        }

    }
}


