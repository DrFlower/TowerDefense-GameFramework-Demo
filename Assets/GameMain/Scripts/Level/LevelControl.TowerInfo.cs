using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Flower.Data;
using GameFramework;

namespace Flower
{
    partial class LevelControl : IReference
    {
        private class TowerInfo : IReference
        {
            public Tower Tower
            {
                get;
                private set;
            }

            public EntityTowerBase EntityTower
            {
                get;
                private set;
            }

            public IPlacementArea PlacementArea
            {
                get;
                private set;
            }

            public IntVector2 PlaceGrid
            {
                get;
                private set;
            }

            public TowerInfo()
            {
                this.Tower = null;
                this.EntityTower = null;
                this.PlacementArea = null;
                this.PlaceGrid = IntVector2.zero;
            }

            public static TowerInfo Create(Tower tower, EntityTowerBase entityTower, IPlacementArea placementArea, IntVector2 placeGrid)
            {
                TowerInfo towerInfo = ReferencePool.Acquire<TowerInfo>();
                towerInfo.Tower = tower;
                towerInfo.EntityTower = entityTower;
                towerInfo.PlacementArea = placementArea;
                towerInfo.PlaceGrid = placeGrid;
                return towerInfo;
            }

            public void Clear()
            {
                this.Tower = null;
                this.EntityTower = null;
                this.PlacementArea = null;
                this.PlaceGrid = IntVector2.zero;
            }
        }
    }

}

