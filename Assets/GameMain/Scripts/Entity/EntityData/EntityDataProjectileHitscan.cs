using System;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    [Serializable]
    public class EntityDataProjectileHitscan : EntityData
    {
        public EntityBaseEnemy EntityEnemy
        {
            get;
            private set;
        }

        public float Damge
        {
            get;
            private set;
        }

        protected Vector3 Origin
        {
            get;
            private set;
        }

        public EntityDataProjectileHitscan() : base()
        {
            EntityEnemy = null;
            Damge = 0;
            Origin = Vector3.zero;
        }

        public static EntityDataProjectileHitscan Create(EntityBaseEnemy EntityEnemy, float damage, Vector3 origin, object userData = null)
        {
            EntityDataProjectileHitscan entityData = ReferencePool.Acquire<EntityDataProjectileHitscan>();
            entityData.EntityEnemy = EntityEnemy;
            entityData.Damge = damage;
            entityData.Origin = origin;
            return entityData;
        }

        public static EntityDataProjectileHitscan Create(EntityBaseEnemy EntityEnemy, float damage, Vector3 origin, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataProjectileHitscan entityData = ReferencePool.Acquire<EntityDataProjectileHitscan>();
            entityData.EntityEnemy = EntityEnemy;
            entityData.Damge = damage;
            entityData.Origin = origin;
            entityData.Position = position;
            entityData.Rotation = rotation;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            EntityEnemy = null;
            Damge = 0;
            Origin = Vector3.zero;
        }
    }
}


