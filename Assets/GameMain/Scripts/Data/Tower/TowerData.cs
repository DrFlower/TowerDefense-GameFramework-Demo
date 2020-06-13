using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace Flower
{
    public class TowerData 
    {
        private DRTower dRTower;
        private DRAssetsPath dRAssetsPath;

        public int Id
        {
            get
            {
                return dRTower.Id;
            }
        }

        public string NameId
        {
            get
            {
                return GameEntry.Localization.GetString(dRTower.NameId);
            }
        }

        public string AssetPath
        {
            get
            {
                return dRAssetsPath.AssetPath;
            }
        }

        public string Icon
        {
            get
            {
                return dRTower.Icon;
            }
        }
    }
}


