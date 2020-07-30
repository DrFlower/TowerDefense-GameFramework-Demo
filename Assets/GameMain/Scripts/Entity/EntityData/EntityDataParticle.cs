using System;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    [Serializable]
    public class EntityDataParticle : EntityData
    {
        public Transform Follow
        {
            get;
            private set;
        }

        public Vector3 Offset
        {
            get;
            private set;
        }

        public Vector3 Scale
        {
            get;
            private set;
        }

        public EntityDataParticle() : base()
        {
            Follow = null;
            Offset = Vector3.zero;
            Scale = Vector3.one;
        }

        public static EntityDataParticle Create(Transform follow, object userData = null)
        {
            EntityDataParticle entityData = ReferencePool.Acquire<EntityDataParticle>();
            entityData.Follow = follow;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataParticle Create(Transform follow, Vector3 offset, object userData = null)
        {
            EntityDataParticle entityData = ReferencePool.Acquire<EntityDataParticle>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataParticle Create(Transform follow, Vector3 offset, Vector3 scale, object userData = null)
        {
            EntityDataParticle entityData = ReferencePool.Acquire<EntityDataParticle>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.Scale = scale;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataParticle Create(Transform follow, Vector3 offset, Vector3 scale, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataParticle entityData = ReferencePool.Acquire<EntityDataParticle>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.Scale = scale;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.UserData = userData;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            Follow = null;
            Offset = Vector3.zero;
            Scale = Vector3.one;
        }
    }
}