using System;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    [Serializable]
    public class EntityDataProjectile : EntityData
    {
        public EntityTargetable EntityTargetable
        {
            get;
            private set;
        }

        public ProjectileData ProjectileData
        {
            get;
            private set;
        }
        public Vector3 Origin
        {
            get;
            private set;
        }

        public Transform FiringPoint
        {
            get;
            private set;
        }

        public EntityDataProjectile() : base()
        {
            EntityTargetable = null;
            Origin = Vector3.zero;
            FiringPoint = null;
        }

        public static EntityDataProjectile Create(EntityTargetable entityTargetable, ProjectileData projectileData, Vector3 origin, Transform firingPoint, object userData = null)
        {
            EntityDataProjectile entityData = ReferencePool.Acquire<EntityDataProjectile>();
            entityData.EntityTargetable = entityTargetable;
            entityData.ProjectileData = projectileData;
            entityData.Origin = origin;
            entityData.FiringPoint = firingPoint;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataProjectile Create(EntityTargetable entityTargetable, ProjectileData projectileData, Vector3 origin, Transform firingPoint, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataProjectile entityData = ReferencePool.Acquire<EntityDataProjectile>();
            entityData.EntityTargetable = entityTargetable;
            entityData.ProjectileData = projectileData;
            entityData.Origin = origin;
            entityData.FiringPoint = firingPoint;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.UserData = userData;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            EntityTargetable = null;
            ProjectileData = null;
            Origin = Vector3.zero;
            FiringPoint = null;
        }
    }
}


