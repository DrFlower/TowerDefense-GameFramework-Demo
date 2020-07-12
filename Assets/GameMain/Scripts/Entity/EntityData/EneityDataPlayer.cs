using System;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    [Serializable]
    public class EneityDataPlayer : EntityData
    {
        public EneityDataPlayer() : base()
        {
            
        }

        //public static EneityDataPlayer Create(object userData = null)
        //{
        //    EneityDataPlayer entityData = ReferencePool.Acquire<EneityDataPlayer>();

        //    return entityData;
        //}

        //public static EneityDataPlayer Create( Vector3 position, Quaternion rotation, object userData = null)
        //{
        //    EneityDataPlayer entityData = ReferencePool.Acquire<EneityDataPlayer>();

        //    entityData.Position = position;
        //    entityData.Rotation = rotation;
        //    return entityData;
        //}

        public override void Clear()
        {
            base.Clear();
        }
    }
}


