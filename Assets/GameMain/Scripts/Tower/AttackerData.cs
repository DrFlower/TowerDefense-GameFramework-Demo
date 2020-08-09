using System;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class AttackerData : IReference
    {
        public float Range
        {
            get;
            private set;
        }

        public float FireRate
        {
            get;
            private set;
        }

        public bool IsMultiAttack
        {
            get;
            private set;
        }

        public string ProjectileType
        {
            get;
            private set;
        }

        public int ProjectileEntityId
        {
            get;
            private set;
        }

        public AttackerData()
        {
            this.Range = 0;
            this.FireRate = 0;
            this.IsMultiAttack = false;
            this.ProjectileType = null;
            this.ProjectileEntityId = -1;
        }


        public static AttackerData Create(float range, float fireRate, bool isMultiAttack, string projectileType, int projectileEntityId)
        {
            AttackerData attackerData = ReferencePool.Acquire<AttackerData>();
            attackerData.Range = range;
            attackerData.FireRate = fireRate;
            attackerData.IsMultiAttack = isMultiAttack;
            attackerData.ProjectileType = projectileType;
            attackerData.ProjectileEntityId = projectileEntityId;
            return attackerData;
        }

        public void Clear()
        {
            this.Range = 0;
            this.FireRate = 0;
            this.IsMultiAttack = false;
            this.ProjectileType = null;
            this.ProjectileEntityId = -1;
        }
    }
}
