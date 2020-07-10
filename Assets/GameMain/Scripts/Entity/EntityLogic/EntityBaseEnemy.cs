using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Flower.Data;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class EntityBaseEnemy : EntityLogicEx
    {
        private LevelPath levelPath;
        private int targetPathNodeIndex;
        private NavMeshAgent agent;

        protected EntityDataEnemy entityDataEnemy;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            agent = GetComponent<NavMeshAgent>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (levelPath.PathNodes.Length > targetPathNodeIndex)
            {
                agent.SetDestination(levelPath.PathNodes[targetPathNodeIndex].position);

                if (Vector3.Distance(transform.position, levelPath.PathNodes[targetPathNodeIndex].position) < 0.5f)
                {
                    targetPathNodeIndex++;
                }
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDataEnemy = userData as EntityDataEnemy;

            if (entityDataEnemy == null)
            {
                Log.Error("Entity enemy '{0}' entity data vaild.", Id);
                return;
            }

            levelPath = entityDataEnemy.LevelPath;

            targetPathNodeIndex = 0;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            targetPathNodeIndex = 0;
        }
    }
}


