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
        private HPBar hpBar;

        private Vector3 m_CurrentPosition, m_PreviousPosition;
        private float hp;
        private bool attacked = false;
        private float attackTimer = 0;
        private EntityPlayer targetPlayer;

        private Dictionary<int, float> dicSlowDownRates;

        //表示是否死亡或已攻击玩家即将回收，以防重复执行回收逻辑
        private bool hide = false;

        protected bool pause = false;

        private Entity slowDownEffect;

        public EntityDataEnemy EntityDataEnemy
        {
            get;
            private set;
        }

        public Vector3 Velocity
        {
            get;
            private set;
        }

        public float CurrentSlowRate
        {
            get;
            private set;
        }

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
            hpBar = transform.Find("HealthBar").GetComponent<HPBar>();
            dicSlowDownRates = new Dictionary<int, float>();
            CurrentSlowRate = 1;
            hpBar.OnInit(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (targetPlayer != null && !attacked)
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

            agent.speed = EntityDataEnemy.EnemyData.Speed * CurrentSlowRate;

            hpBar.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            EntityDataEnemy = userData as EntityDataEnemy;

            if (EntityDataEnemy == null)
            {
                Log.Error("Entity enemy '{0}' entity data invaild.", Id);
                return;
            }

            agent.enabled = true;
            agent.speed = EntityDataEnemy.EnemyData.Speed * CurrentSlowRate;

            levelPath = EntityDataEnemy.LevelPath;

            targetPathNodeIndex = 0;

            hp = EntityDataEnemy.EnemyData.MaxHP;

            hpBar.OnShow(userData);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            if (OnHidden != null)
                OnHidden(this);

            OnHidden = null;
            OnDead = null;

            levelPath = null;
            EntityDataEnemy = null;
            targetPathNodeIndex = 0;
            hp = 0;
            agent.enabled = false;
            attacked = false;
            attackTimer = 0;
            targetPlayer = null;

            hide = false;

            RemoveSlowEffect();
            dicSlowDownRates.Clear();

            hpBar.OnHide(isShutdown, userData);
        }

        void FixedUpdate()
        {
            m_CurrentPosition = transform.position;
            Velocity = (m_CurrentPosition - m_PreviousPosition) / Time.fixedDeltaTime;
            m_PreviousPosition = m_CurrentPosition;
        }

        public void AfterAttack()
        {
            if (!hide)
            {
                hide = true;
                GameEntry.Event.Fire(this, HideEnemyEventArgs.Create(Id));
            }
        }

        public void Damage(float value)
        {
            if (IsDead)
                return;

            hp -= value;

            if (hp <= 0)
            {
                hp = 0;
                Dead();
            }

            hpBar.UpdateHealth(hp / EntityDataEnemy.EnemyData.MaxHP);
        }

        private void Dead()
        {
            if (OnDead != null)
                OnDead(this);

            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                EntityDataEnemy.EnemyData.DeadEffcetEntityId,
                typeof(EntityParticleAutoHide),
                null,
                EntityData.Create(transform.position + EntityDataEnemy.EnemyData.DeadEffectOffset, transform.rotation)));

            if (!hide)
            {
                hide = true;
                GameEntry.Event.Fire(this, HideEnemyEventArgs.Create(Id));
            }
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

        public void ApplySlow(int towerId, float slowRate)
        {
            if (dicSlowDownRates.ContainsKey(towerId))
            {
                dicSlowDownRates[towerId] = slowRate;
            }
            else
            {
                dicSlowDownRates.Add(towerId, slowRate);
            }

            foreach (var item in dicSlowDownRates)
            {
                CurrentSlowRate = Mathf.Min(CurrentSlowRate, item.Value);
            }

            //Debug.LogError(string.Format("apply slow by tower {0},slow rate: {1},current slow rate:{2}", towerId, slowRate, CurrentSlowRate));

            ApplySlowEffect();
        }

        public void RemoveSlow(int towerId)
        {
            if (dicSlowDownRates.ContainsKey(towerId))
            {
                dicSlowDownRates.Remove(towerId);
                if (dicSlowDownRates.Count == 0)
                {
                    CurrentSlowRate = 1;
                    RemoveSlowEffect();
                }

                //Debug.LogError(string.Format("remove slow by tower {0},current slow rate:{1}", towerId, CurrentSlowRate));
            }
            else
            {
                Log.Error("error");
            }
        }

        private void ApplySlowEffect()
        {
            if (slowDownEffect == null)
            {
                GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create((int)EnumEntity.SlowFx,
                    typeof(EntityAnimation),
                    (entity) =>
                    {
                        slowDownEffect = entity;
                        if (hide)
                        {
                            RemoveSlowEffect();
                        }
                    },
                        EntityDataFollower.Create(transform,
                        EntityDataEnemy.EnemyData.ApplyEffectOffset,
                        Vector3.one * EntityDataEnemy.EnemyData.ApplyEffectScale,
                        transform.position,
                        transform.rotation)
                    )
                    );
            }
        }

        private void RemoveSlowEffect()
        {
            if (slowDownEffect != null)
            {
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(slowDownEffect.Id));
                slowDownEffect = null;
            }
        }


        public void Pause()
        {
            pause = true;
            agent.speed = 0;
        }

        public void Resume()
        {
            pause = false;
            agent.speed = EntityDataEnemy.EnemyData.Speed * CurrentSlowRate;
        }
    }
}


