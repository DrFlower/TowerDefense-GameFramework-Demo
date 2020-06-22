using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace Flower.Data
{
    public enum EnumLevelState : byte
    {
        None,
        Loading,
        Prepare,
        Normal,
        Pause,
        Gameover
    }
}
