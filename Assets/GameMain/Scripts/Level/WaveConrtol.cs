using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;
using GameFramework;
using GameFramework.Event;

namespace Flower
{
    partial class WaveControl : IReference
    {
        private LevelPathManager levelPathManager;
        private Queue<WaveInfo> waveInfos;
        private DataLevel dataLevel;
        private DataEnemy dataEnemy;

        private EntityLoader entityLoader;

        private int currentWave = 0;
        private int totalWave = 0;

        private float spawnEnemyTimer = 0;
        private float updateWaveInfoTimer = 0;

        private static readonly float UPDATE_WAVE_INFO_RATE = 0.5f;

        public WaveControl()
        {
            waveInfos = new Queue<WaveInfo>();
            currentWave = 1;
            totalWave = 0;
            spawnEnemyTimer = 0;
            updateWaveInfoTimer = 0;
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (dataLevel.LevelState == EnumLevelState.Normal)
            {
                spawnEnemyTimer += elapseSeconds;
                updateWaveInfoTimer += elapseSeconds;

                if (waveInfos.Count > 0)
                {
                    WaveInfo waveInfo = waveInfos.Peek();

                    if (updateWaveInfoTimer >= UPDATE_WAVE_INFO_RATE)
                    {
                        GameEntry.Event.Fire(this, WaveInfoUpdateEventArgs.Create(currentWave, totalWave, spawnEnemyTimer / waveInfo.TotalTime));
                        updateWaveInfoTimer -= UPDATE_WAVE_INFO_RATE;
                    }

                    int result = waveInfo.DequeueEnemy(spawnEnemyTimer);
                    if (result == -1)
                    {
                        waveInfo = waveInfos.Dequeue();
                        spawnEnemyTimer -= waveInfo.TotalTime;
                        ReferencePool.Release(waveInfo);
                        waveInfo = null;
                        currentWave++;
                    }
                    else if (result == 0)
                    {

                    }
                    else
                    {
                        SpawnEnemy(result);
                    }
                }
                else
                {
                    dataLevel.GameSuccess();
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

            entityLoader.ShowEntity<EntityBaseEnemy>(enemyData.EntityId, null,
                EntityDataEnemy.Create(
                    enemyData,
                    levelPathManager.GetLevelPath(),
                    levelPathManager.GetStartPathNode().position - new Vector3(0, 0.2f, 0),
                    Quaternion.identity));
        }

        public void StartWave()
        {
            GameEntry.Event.Subscribe(HideEnemyEventArgs.EventId, HideEnemyEntity);
        }

        public void OnPause()
        {
            var entities = entityLoader.GetAllEntities();
            foreach (var eneity in entities)
            {
                IPause iPause = eneity.Logic as IPause;
                if (iPause == null)
                    continue;

                iPause.Pause();
            }

        }

        public void OnResume()
        {
            var entities = entityLoader.GetAllEntities();
            foreach (var eneity in entities)
            {
                IPause iPause = eneity.Logic as IPause;
                if (iPause == null)
                    continue;

                iPause.Resume();
            }
        }

        public void OnGameover()
        {
            OnPause();
        }

        public void OnQuick()
        {
            OnResume();
            GameEntry.Event.Unsubscribe(HideEnemyEventArgs.EventId, HideEnemyEntity);
            entityLoader.HideAllEntity();
        }

        private void HideEnemyEntity(object sender, GameEventArgs e)
        {
            HideEnemyEventArgs ne = (HideEnemyEventArgs)e;
            if (ne == null)
                return;

            entityLoader.HideEntity(ne.EntityId);
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
            waveControl.currentWave = 1;
            waveControl.totalWave = waveDatas.Length;
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
            currentWave = 1;
            totalWave = 0;
            spawnEnemyTimer = 0;
            updateWaveInfoTimer = 0;

            if (entityLoader != null)
                ReferencePool.Release(entityLoader);
        }
    }
}
