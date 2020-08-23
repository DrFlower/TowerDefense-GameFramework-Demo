using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace Flower.Data
{
    public sealed class LevelData
    {
        private DRLevel dRLevel;
        private WaveData[] waveData;
        private SceneData sceneData;
        private string name;
        private string description;

        public int Id
        {
            get
            {
                return dRLevel.Id;
            }
        }

        public string Name
        {
            get
            {
                return GameEntry.Localization.GetString(dRLevel.NameId);
            }
        }

        public string Description
        {
            get
            {
                return GameEntry.Localization.GetString(dRLevel.DescriptionId);
            }
        }

        public string ResourceGroupName
        {
            get
            {
                return dRLevel.ResourceGroupName;
            }
        }

        public int InitEnergy
        {
            get
            {
                return dRLevel.InitEnergy;
            }
        }

        public Vector3 PlayerPosition
        {
            get
            {
                return dRLevel.PlayerPosition;
            }
        }

        public Quaternion PlayerQuaternion
        {
            get
            {
                return Quaternion.Euler(dRLevel.PlayerQuaternion);
            }
        }

        public WaveData[] WaveDatas
        {
            get
            {
                return waveData;
            }
        }

        public int[] AllowTowers
        {
            get
            {
                return dRLevel.AllowTowers;
            }
        }

        public SceneData SceneData
        {
            get
            {
                return sceneData;
            }
        }

        public LevelData(DRLevel dRLevel, WaveData[] waveData, SceneData sceneData)
        {
            this.dRLevel = dRLevel;
            this.waveData = waveData;
            this.sceneData = sceneData;
        }
    }
}
