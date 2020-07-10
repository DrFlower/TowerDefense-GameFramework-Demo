using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;
using GameFramework;

namespace Flower
{
    partial class WaveControl : IReference
    {
        private LevelPathManager levelPathManager;
        private Queue<WaveInfo> waveInfos;
        private DataLevel dataLevel;
        private DataEnemy dataEnemy;

        private EntityLoader entityLoader;

        private float timer = 0;

        public WaveControl()
        {
            waveInfos = new Queue<WaveInfo>();
            timer = 0;
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (dataLevel.LevelState == EnumLevelState.Normal)
            {
                timer += elapseSeconds;

                if (waveInfos.Count > 0)
                {
                    WaveInfo waveInfo = waveInfos.Peek();
                    int result = waveInfo.DequeueEnemy(timer);
                    if (result == -1)
                    {
                        waveInfos.Dequeue();
                    }
                    else if (result == 0)
                    {

                    }
                    else
                    {
                        Log.Info(string.Format("show enemy {0},at time:{1}", result, timer));
                        SpawnEnemy(result);
                    }
                }
            }
        }

        private void SpawnEnemy(int enemyId)
        {
            EnemyData enemyData = dataEnemy.GetEnemyData(enemyId);

            if (enemyData == null)
            {
                Log.Error("Can not get enemy data by id '{0}'.", enemyId);
                return;
            }

            entityLoader.ShowEntity<EntityBaseEnemy>(enemyData.EntityId, null, EntityDataEnemy.Create(enemyData, levelPathManager.GetLevelPath(), levelPathManager.GetStartPathNode().position-new Vector3(0,0.2f,0), Quaternion.identity));
        }

        public void StartWave()
        {

        }

        public void OnPause()
        {

        }

        public void OnResume()
        {

        }

        public void OnRestart()
        {

        }

        public void OnGameover()
        {

        }

        public void OnQuick()
        {

        }

        public static WaveControl Create(WaveData[] waveDatas, LevelPathManager levelPathManager)
        {
            WaveControl waveControl = ReferencePool.Acquire<WaveControl>();
            for (int i = 0; i < waveDatas.Length; i++)
            {
                waveControl.waveInfos.Enqueue(WaveInfo.Create(waveDatas[i]));
            }

            waveControl.levelPathManager = levelPathManager;
            waveControl.dataLevel = GameEntry.Data.GetData<DataLevel>();
            waveControl.dataEnemy = GameEntry.Data.GetData<DataEnemy>();

            waveControl.entityLoader = EntityLoader.Create(waveControl);

            return waveControl;
        }

        public void Clear()
        {
            while (waveInfos.Count > 0)
            {
                WaveInfo waveInfo = waveInfos.Dequeue();
                ReferencePool.Release(waveInfo);
            }

            levelPathManager = null;

            waveInfos.Clear();
            dataLevel = null;
            timer = 0;

            if (entityLoader != null)
                ReferencePool.Release(entityLoader);
        }
    }
}
