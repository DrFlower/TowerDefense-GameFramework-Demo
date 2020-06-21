using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;

namespace Flower
{
    public static class EntityExtension 
    {
        private static int s_SerialId = 0;



        public static void ShowEntity<T>(this EntityComponent entityComponent, int serialId, EnumEntity enumEntity, object userData = null)
        {
            entityComponent.ShowEntity(serialId, enumEntity, typeof(T), userData);
        }

        public static void ShowEntity(this EntityComponent entityComponent, int serialId, EnumEntity enumEntity, Type logicType, object userData = null)
        {
            entityComponent.ShowEntity(serialId, (int)enumEntity, logicType, userData);
        }

        public static void ShowEntity<T>(this EntityComponent entityComponent, int serialId, int entityId, object userData = null)
        {
            entityComponent.ShowEntity(serialId, entityId, typeof(T), userData);
        }

        public static void ShowEntity(this EntityComponent entityComponent, int serialId, int entityId, Type logicType, object userData = null)
        {
            EntityData entityData = GameEntry.Data.GetData<DataEntity>().GetEntityData(entityId);

            if (entityData == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", entityData.Id.ToString());
                return;
            }

            entityComponent.ShowEntity(serialId, logicType, entityData.AssetPath, entityData.EntityGroupData.Name, Constant.AssetPriority.EntityAsset, userData);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return ++s_SerialId;
        }

    }

}