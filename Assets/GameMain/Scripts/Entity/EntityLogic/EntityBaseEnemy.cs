using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Flower.Data;
using UnityGameFramework.Runtime;
using System;

namespace Flower
{
    public class EntityBaseEnemy : EntityLogicEx, IPause
    {
        private LevelPath levelPath;
        private int targetPathNodeIndex;
        private NavMeshAgent agent;

        protected EntityDataEnemy entityDataEnemy;

        private float hp;
        private bool attacked = false;
        private float attackTimer = 0;
        private EntityPlayer targetPlayer;

        protected bool pause = false;

        public bool IsDead
        {
            get
            {
                return hp <= 0;
            }
        }

        public event Action<EntityBaseEnemy> OnDead;

        public event Action<EntityBaseEnemy> OnHidden;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            agent = GetComponent<NavMeshAgent>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (!pause && targetPlayer != null && !attacked)
            {
                attackTimer += elapseSeconds;
                if (attackTimer > 1)
                {
                    targetPlayer.Damage();
                    attacked = true;
                    AfterAttack();
                }
            }

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
                Log.Error("Entity enemy '{0}' entity data invaild.", Id);
                return;
            }

            agent.enabled = true;
            agent.speed = entityDataEnemy.EnemyData.Speed;

            levelPath = entityDataEnemy.LevelPath;

            targetPathNodeIndex = 0;

            hp = entityDataEnemy.EnemyData.MaxHP;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            if (OnHidden != null)
                OnHidden(this);

            OnHidden = null;
            OnDead = null;

            levelPath = null;
            entityDataEnemy = null;
            targetPathNodeIndex = 0;
            hp = 0;
            agent.enabled = false;
            attacked = false;
            attackTimer = 0;
            targetPlayer = null;
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
            if (OnDead != null)
                OnDead(this);

            GameEntry.Event.Fire(this, HideEnemyEventArgs.Create(Id));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (attacked)
                return;

            var player = other.GetComponent<EntityPlayer>();
            if (player == null)
            {
                return;
            }
            targetPlayer = player;

            player.Charge();
        }

        public void Pause()
        {
            pause = true;
            agent.speed = 0;
        }

        public void Resume()
        {
            pause = false;
            agent.speed = entityDataEnemy.EnemyData.Speed;
        }
    }
}


