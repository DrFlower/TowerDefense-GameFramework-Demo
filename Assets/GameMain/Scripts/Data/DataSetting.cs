using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;

namespace Flower
{
    public class DataSetting : DataBase
    {
        public override void Init()
        {
            Debug.LogError("DataSetting Init");
        }

        public override void OnPreload()
        {
            Debug.LogError("DataSetting OnPreload");
        }

        public override void OnLoad()
        {
            Debug.LogError("DataSetting OnLoad");
        }

        public override void OnUnload()
        {
            Debug.LogError("DataSetting OnUnload");
        }

        public override void Shutdown()
        {
            Debug.LogError("DataSetting Shutdown");
        }
    }

}