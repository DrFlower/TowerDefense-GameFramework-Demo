using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using GameFramework;

namespace Flower.Data
{
    public sealed class DataLevel : DataBase
    {
        private IDataTable<DRLevel> dtLevel;
        private Dictionary<int, LevelData> dicLevelData;

        private int[] starScore;

        private readonly static int NONE_LEVEL_INDEX = -1;

        private EnumLevelState stateBeforePause;

        public EnumLevelState LevelState
        {
            get;
            private set;
        }

        public Level CurrentLevel
        {
            get;
            private set;
        }

        public int CurrentLevelIndex
        {
            get;
            private set;
        }

        public int MaxLevel
        {
            get;
            private set;
        }

        public bool IsInLevel
        {
            get
            {
                return CurrentLevelIndex != NONE_LEVEL_INDEX;
            }
        }

        protected override void OnInit()
        {
            LevelState = EnumLevelState.None;
            CurrentLevelIndex = NONE_LEVEL_INDEX;
        }

        protected override void OnPreload()
        {
            LoadDataTable("Level");
        }

        protected override void OnLoad()
        {
            MaxLevel = 0;

            dtLevel = GameEntry.DataTable.GetDataTable<DRLevel>();
            if (dtLevel == null)
                throw new System.Exception("Can not get data table Level");

            dicLevelData = new Dictionary<int, LevelData>();

            DataWave dataWave = GameEntry.Data.GetData<DataWave>();
            if (dataWave == null)
                throw new System.Exception("Can not get data 'DataWave'");

            DRLevel[] dRLevels = dtLevel.GetAllDataRows();
            foreach (var dRLevel in dRLevels)
            {
                SceneData sceneData = GameEntry.Data.GetData<DataScene>().GetSceneData(dRLevel.SceneId);

                int[] waveIds = dRLevel.WaveIds;
                WaveData[] waveDatas = new WaveData[waveIds.Length];
                for (int i = 0; i < waveIds.Length; i++)
                {
                    WaveData waveData = dataWave.GetWaveData(waveIds[i]);
                    if (waveData == null)
                        throw new System.Exception("Can not find Wave Data id :" + waveIds[i]);

                    waveDatas[i] = waveData;
                }

                LevelData levelData = new LevelData(dRLevel, waveDatas, sceneData);
                dicLevelData.Add(dRLevel.Id, levelData);

                if (dRLevel.Id > MaxLevel)
                    MaxLevel = dRLevel.Id;
            }

            //
            starScore = new int[3];
            starScore[0] = GameEntry.Config.GetInt(Constant.Config.LevelStar1);
            starScore[1] = GameEntry.Config.GetInt(Constant.Config.LevelStar2);
            starScore[2] = GameEntry.Config.GetInt(Constant.Config.LevelStar3);

            Subscribe(LoadLevelFinishEventArgs.EventId, OnLoadLevelFinfish);
        }

        public LevelData GetLevelData(int id)
        {
            if (dicLevelData.ContainsKey(id))
            {
                return dicLevelData[id];
            }

            return null;
        }

        public LevelData[] GetAllLevelData()
        {
            int index = 0;
            LevelData[] results = new LevelData[dicLevelData.Count];
            foreach (var levelData in dicLevelData.Values)
            {
                results[index++] = levelData;
            }

            return results;
        }

        private void ChangeLevelState(EnumLevelState targetLevelState)
        {
            if (LevelState == targetLevelState)
                return;

            LevelData levelData = GetLevelData(CurrentLevelIndex);
            if (levelData == null)
            {
                Log.Error("Can not found level '{0}.'", CurrentLevelIndex);
                return;
            }

            EnumLevelState lastLevelState = LevelState;
            LevelState = targetLevelState;
            GameEntry.Event.Fire(this, LevelStateChangeEventArgs.Create(levelData, lastLevelState, LevelState));
            Log.Debug("Current level is '{0}',level state is '{1}.'", levelData.Name, LevelState.ToString());
        }

        public void LoadLevel(int level)
        {
            if (LevelState == EnumLevelState.Loading)
            {
                Log.Error("Can not load level when loading level.");
                return;
            }

            if (level <= 0)
            {
                Log.Error("Load level param invaild '{0}.'", level);
                return;
            }

            if (!dicLevelData.ContainsKey(level))
            {
                Log.Error("Can not found level '{0}.'", level);
                return;
            }

            LevelData levelData = dicLevelData[level];

            InternalLoadLevel(levelData);
        }

        private void InternalLoadLevel(LevelData levelData)
        {
            bool isReload = true;

            if (CurrentLevelIndex != levelData.Id)
            {
                CurrentLevelIndex = levelData.Id;
                isReload = false;
            }

            if (CurrentLevel != null)
                ReferencePool.Release(CurrentLevel);

            CurrentLevel = Level.Create(levelData);

            GameEntry.Data.GetData<DataPlayer>().Reset();

            ChangeLevelState(isReload ? EnumLevelState.Prepare : EnumLevelState.Loading);

            if (isReload)
                GameEntry.Event.Fire(this, ReloadLevelEventArgs.Create(levelData));
            else
                GameEntry.Event.Fire(this, LoadLevelEventArgs.Create(levelData));
        }

        public void StartWave()
        {
            if (CurrentLevelIndex == NONE_LEVEL_INDEX)
            {
                Log.Error("Only can start wave in level");
                return;
            }

            if (LevelState != EnumLevelState.Prepare)
            {
                Log.Error("Only can start wave when level is in Prepare State,now is {0}", LevelState.ToString());
                return;
            }
            ChangeLevelState(EnumLevelState.Normal);
            GameEntry.Event.Fire(this, StartWaveEventArgs.Create());
        }

        public void LevelPause()
        {
            if (CurrentLevelIndex == NONE_LEVEL_INDEX)
            {
                Log.Error("Only can pause in level");
                return;
            }

            if (LevelState != EnumLevelState.Normal && LevelState != EnumLevelState.Prepare)
            {
                Log.Error("Only can pause when level is in Normal or Prepare State,now is {0}", LevelState.ToString());
                return;
            }

            stateBeforePause = LevelState;
            ChangeLevelState(EnumLevelState.Pause);
        }

        public void LevelResume()
        {
            if (CurrentLevelIndex == NONE_LEVEL_INDEX)
            {
                Log.Error("Only can resume in level");
                return;
            }

            if (LevelState != EnumLevelState.Pause)
            {
                Log.Error("Only can resume when level is in Pause State,now is {0}", LevelState.ToString());
                return;
            }

            ChangeLevelState(stateBeforePause);
        }

        public void ExitLevel()
        {
            if (CurrentLevelIndex != NONE_LEVEL_INDEX)
            {
                if (CurrentLevel != null)
                    ReferencePool.Release(CurrentLevel);
                CurrentLevel = null;

                ChangeLevelState(EnumLevelState.None);
                CurrentLevelIndex = NONE_LEVEL_INDEX;
                GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Menu")));
            }

        }

        public void GameSuccess()
        {
            if (LevelState == EnumLevelState.Gameover)
                return;

            if (CurrentLevelIndex == NONE_LEVEL_INDEX)
            {
                Log.Error("Gameover Only heppen in level");
                return;
            }

            if (LevelState != EnumLevelState.Normal)
            {
                Log.Error("Gameover Only heppen when level is in Normal State,now is {0}", LevelState.ToString());
                return;
            }

            DataPlayer dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            int hp = dataPlayer.HP;
            int starCount = 0;
            for (int i = 0; i < starScore.Length; i++)
            {
                if (hp >= starScore[i])
                {
                    starCount = i + 1;
                }
                else
                {
                    starCount = i;
                    break;
                }
            }

            SetLevelRecord(CurrentLevelIndex, starCount);
            ChangeLevelState(EnumLevelState.Gameover);
            GameEntry.Event.Fire(this, GameoverEventArgs.Create(EnumGameOverType.Success, starCount));
        }

        public void GameFail()
        {
            if (LevelState == EnumLevelState.Gameover)
                return;

            if (CurrentLevelIndex == NONE_LEVEL_INDEX)
            {
                Log.Error("Gameover Only heppen in level");
                return;
            }

            if (LevelState != EnumLevelState.Normal)
            {
                Log.Error("Gameover Only heppen when level is in Normal State,now is {0}", LevelState.ToString());
                return;
            }

            ChangeLevelState(EnumLevelState.Gameover);
            GameEntry.Event.Fire(this, GameoverEventArgs.Create(EnumGameOverType.Fail, 0));
        }

        private void SetLevelRecord(int levelIndex, int starCount)
        {
            int currentStarCount = GameEntry.Setting.GetInt(string.Format(Constant.Setting.LevelStarRecord, levelIndex), 0);
            if (starCount > currentStarCount)
            {
                GameEntry.Setting.SetInt(string.Format(Constant.Setting.LevelStarRecord, levelIndex), starCount);
            }
        }

        private void OnLoadLevelFinfish(object sender, GameEventArgs e)
        {
            LoadLevelFinishEventArgs ne = (LoadLevelFinishEventArgs)e;
            if (ne == null)
            {
                return;
            }

            if (LevelState != EnumLevelState.Loading)
            {
                return;
            }

            LevelData levelData = GetLevelData(CurrentLevelIndex);
            if (levelData != null && levelData.SceneData.Id == ne.LevelId)
            {
                ChangeLevelState(EnumLevelState.Prepare);
            }
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRLevel>();

            dtLevel = null;
            dicLevelData = null;

            if (CurrentLevel != null)
            {
                ReferencePool.Release(CurrentLevel);
                CurrentLevel = null;
            }

            LevelState = EnumLevelState.None;
            CurrentLevelIndex = NONE_LEVEL_INDEX;
        }

        protected override void OnShutdown()
        {
        }
    }

}