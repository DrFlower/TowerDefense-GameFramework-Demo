using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Flower.Data;

namespace Flower
{
    public interface ILauncher
    {

        void Launch(EntityTargetable target, AttackerData attackerData, ProjectileData projectileData, Vector3 origin, Transform firingPoint);

        void Launch(EntityTargetable target, AttackerData attackerData, ProjectileData projectileData, Vector3 origin, Transform[] firingPoints);

        void Launch(List<EntityTargetable> targets, AttackerData attackerData, ProjectileData projectileData, Vector3 origin, Transform[] firingPoints);
    }
}
