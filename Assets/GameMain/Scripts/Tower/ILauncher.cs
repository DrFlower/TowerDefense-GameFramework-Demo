using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Flower.Data;

namespace Flower
{
    public interface ILauncher
    {

        void Launch(EntityBaseEnemy enemy, Tower tower, Vector3 origin, Transform firingPoint);

        void Launch(EntityBaseEnemy enemy, Tower tower, Vector3 origin, Transform[] firingPoints);

        void Launch(List<EntityBaseEnemy> enemies, Tower tower, Vector3 origin, Transform[] firingPoints);
    }
}
