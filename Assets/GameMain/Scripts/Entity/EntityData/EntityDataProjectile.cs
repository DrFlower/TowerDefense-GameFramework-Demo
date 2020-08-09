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

        //public int Level
        //{
        //    get;
        //    private set;
        //}

        //public float Damage
        //{
        //    get;
        //    private set;
        //}

        //public float SplashDamage
        //{
        //    get;
        //    private set;
        //}

        //public float SplashRange
        //{
        //    get;
        //    private set;
        //}

        //public float FireRate
        //{
        //    get;
        //    private set;
        //}

        //public float SpeedDownRate
        //{
        //    get;
        //    private set;
        //}

        //public float EnergyRaise
        //{
        //    get;
        //    private set;
        //}

        //public float EnergyRaiseRate
        //{
        //    get;
        //    private set;
        //}

        //public float Range
        //{
        //    get;
        //    private set;
        //}

        //public int ProjectileEntityId
        //{
        //    get;
        //    private set;
        //}

        //public string ProjectileType
        //{
        //    get;
        //    private set;
        //}

        //public bool IsMultiAttack
        //{
        //    get;
        //    private set;
        //}

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
            // Level = 0;
            //Damage = 0;
            //SplashDamage = 0;
            //SplashRange = 0;
            //FireRate = 0;
            //SpeedDownRate = 0;
            //EnergyRaise = 0;
            //EnergyRaiseRate = 0;
            //Range = 0;
            //ProjectileEntityId = 0;
            //ProjectileType = null;
            Origin = Vector3.zero;
            FiringPoint = null;
        }

        public static EntityDataProjectile Create(EntityTargetable entityTargetable, ProjectileData projectileData, Vector3 origin, Transform firingPoint, object userData = null)
        {
            EntityDataProjectile entityData = ReferencePool.Acquire<EntityDataProjectile>();
            entityData.EntityTargetable = entityTargetable;
            entityData.ProjectileData = projectileData;
            //entityData.Level = tower.Level;
            //entityData.Damage = tower.Damage;
            //entityData.SplashDamage = tower.SplashDamage;
            //entityData.SplashRange = tower.SplashRange;
            //entityData.FireRate = tower.FireRate;
            //entityData.SpeedDownRate = tower.SpeedDownRate;
            //entityData.EnergyRaise = tower.EnergyRaise;
            //entityData.EnergyRaiseRate = tower.EnergyRaiseRate;
            //entityData.Range = tower.Range;
            //entityData.ProjectileEntityId = tower.ProjectileEntityId;
            //entityData.ProjectileType = tower.ProjectileType;
            //entityData.IsMultiAttack = tower.IsMultiAttack;
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
            //entityData.Level = tower.Level;
            //entityData.Damage = tower.Damage;
            //entityData.SplashDamage = tower.SplashDamage;
            //entityData.SplashRange = tower.SplashRange;
            //entityData.FireRate = tower.FireRate;
            //entityData.SpeedDownRate = tower.SpeedDownRate;
            //entityData.EnergyRaise = tower.EnergyRaise;
            //entityData.EnergyRaiseRate = tower.EnergyRaiseRate;
            //entityData.Range = tower.Range;
            //entityData.ProjectileEntityId = tower.ProjectileEntityId;
            //entityData.ProjectileType = tower.ProjectileType;
            //entityData.IsMultiAttack = tower.IsMultiAttack;
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
            //Level = 0;
            //Damage = 0;
            //SplashDamage = 0;
            //SplashRange = 0;
            //FireRate = 0;
            //SpeedDownRate = 0;
            //EnergyRaise = 0;
            //EnergyRaiseRate = 0;
            //Range = 0;
            //ProjectileEntityId = 0;
            //ProjectileType = null;
            Origin = Vector3.zero;
            FiringPoint = null;
        }
    }
}


