using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace Flower.Data
{
    public class EnemyData
    {
        private DREnemy dREnemy;
        private ProjectileData projectileData;

        public int Id
        {
            get
            {
                return dREnemy.Id;
            }
        }

        public string NameId
        {
            get
            {
                return GameEntry.Localization.GetString(dREnemy.NameId);
            }
        }

        public int EntityId
        {
            get
            {
                return dREnemy.EntityId;
            }
        }

        public string Type
        {
            get
            {
                return dREnemy.Type;
            }
        }

        public float MaxHP
        {
            get
            {
                return dREnemy.MaxHP;
            }
        }

        public int Damage
        {
            get
            {
                return dREnemy.Damage;
            }
        }

        public int ProjectileEntityId
        {
            get
            {
                return dREnemy.ProjectileEntityId;
            }
        }

        public string ProjectileType
        {
            get
            {
                return dREnemy.ProjectileType;
            }
        }

        public ProjectileData ProjectileData
        {
            get
            {
                return projectileData;
            }
        }

        public int ProjectileDataId
        {
            get
            {
                return dREnemy.ProjectileData;
            }
        }

        public float FireRate
        {
            get
            {
                return dREnemy.FireRate;
            }
        }

        public float Range
        {
            get
            {
                return dREnemy.Range;
            }
        }

        public bool IsMultiAttack
        {
            get
            {
                return dREnemy.IsMultiAttack;
            }
        }

        public float AddEnergy
        {
            get
            {
                return dREnemy.AddEnergy;
            }
        }

        public float Speed
        {
            get
            {
                return dREnemy.Speed;
            }
        }

        public EnemyData(DREnemy dREnemy, ProjectileData projectileData)
        {
            this.dREnemy = dREnemy;
            this.projectileData = projectileData;
        }

    }
}


