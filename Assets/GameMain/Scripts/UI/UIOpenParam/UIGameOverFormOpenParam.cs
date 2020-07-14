using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Flower.Data;

namespace Flower
{
    public class UIGameOverFormOpenParam : IReference
    {
        public LevelData LevelData
        {
            get;
            private set;
        }

        public EnumGameOverType EnumGameOverType
        {
            get;
            private set;
        }

        public int StarCount
        {
            get;
            private set;
        }

        public UIGameOverFormOpenParam()
        {
            LevelData = null;
            EnumGameOverType = EnumGameOverType.Fail;
            StarCount = 0;
        }

        public static UIGameOverFormOpenParam Create(LevelData levelData, EnumGameOverType enumGameOverType, int starCount)
        {
            UIGameOverFormOpenParam uIGameOverFormOpenParam = ReferencePool.Acquire<UIGameOverFormOpenParam>();
            uIGameOverFormOpenParam.LevelData = levelData;
            uIGameOverFormOpenParam.EnumGameOverType = enumGameOverType;
            uIGameOverFormOpenParam.StarCount = starCount;
            return uIGameOverFormOpenParam;
        }


        public void Clear()
        {
            LevelData = null;
            EnumGameOverType = EnumGameOverType.Fail;
            StarCount = 0;
        }
    }
}