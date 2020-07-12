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

        private float hp;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            agent = GetComponent<NavMeshAgent>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (levelPath == null || levelPath.PathNodes == null || levelPath.PathNodes.Length == 0)
                return;

            if (levelPath.PathNodes.Length > targetPathNodeIndex)
            {
                agent.SetDestination(levelPath.PathNodes[targetPathNodeIndex].position);
                if (Vector3.Distance(transform.position, levelPath.PathNodes[targetPathNodeIndex].position) < 1f)
                {
                    if (levelPath.PathNodes.Length - 1 == targetPathNodeIndex)
                    {
                        agent.isStopped = true;
                    }
                    else
                    {
                        targetPathNodeIndex++;
                    }
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

            agent.enabled = true;
            agent.speed = entityDataEnemy.EnemyData.Speed;

            levelPath = entityDataEnemy.LevelPath;

            targetPathNodeIndex = 0;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);


            levelPath = null;
            entityDataEnemy = null;
            targetPathNodeIndex = 0;
            hp = 0;
            agent.enabled = false;
        }

        public void AfterAttack()
        {
            GameEntry.Event.Fire(this, HideEnemyEventArgs.Create(Id));
        }

        public void Damage(float value)
        {
            hp -= value;

            if (hp <= 0)
            {
                hp = 0;
                Dead();
            }
        }

        private void Dead()
        {

        }
    }
}


