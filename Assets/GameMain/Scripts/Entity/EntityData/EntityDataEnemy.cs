using System;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    [Serializable]
    public class EntityDataEnemy : EntityData
    {
        public EnemyData EnemyData
        {
            get;
            private set;
        }

        public LevelPath LevelPath
        {
            get;
            private set;
        }

        public EntityDataEnemy() : base()
        {
            EnemyData = null;
        }

        public static EntityDataEnemy Create(EnemyData enemyData, LevelPath levelPath, object userData = null)
        {
            EntityDataEnemy entityData = ReferencePool.Acquire<EntityDataEnemy>();
            entityData.EnemyData = enemyData;
            entityData.LevelPath = levelPath;
            return entityData;
        }

        public static EntityDataEnemy Create(EnemyData enemyData, LevelPath levelPath, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataEnemy entityData = ReferencePool.Acquire<EntityDataEnemy>();
            entityData.EnemyData = enemyData;
            entityData.LevelPath = levelPath;
            entityData.Position = position;
            entityData.Rotation = rotation;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            EnemyData = null;
        }
    }
}


