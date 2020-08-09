using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower.Data
{
    public class ProjectileData
    {
        private DRProjectile dRProjectile;

        public int Id
        {
            get
            {
                return dRProjectile.Id;
            }
        }

        public float Damage
        {
            get
            {
                return dRProjectile.Damage;
            }
        }

        public float SplashDamage
        {
            get
            {
                return dRProjectile.SplashDamage;
            }
        }

        public float SplashRange
        {
            get
            {
                return dRProjectile.SplashRange;
            }
        }

        public ProjectileData(DRProjectile dRProjectile)
        {
            this.dRProjectile = dRProjectile;
        }
    }

}

