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
        private Transform hpBarRoot;
        private EntityHPBar entityHPBar;

        private Vector3 m_CurrentPosition, m_PreviousPosition;
        private float hp;
        private bool attacked = false;
        private float attackTimer = 0;
        private EntityPlayer targetPlayer;

        private DataPlayer dataPlayer;

        private Dictionary<int, float> dicSlowDownRates;

        //表示是否死亡或已攻击玩家即将回收，以防重复执行回收逻辑
        private bool hide = false;
        private bool loadedHPBar = false;

        protected bool pause = false;

        private Entity slowDownEffect;
        private bool loadSlowDownEffect = false;

        private RandomSound randomSound;

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
            hpBarRoot = transform.Find("HealthBar");
            randomSound = GetComponent<RandomSound>();
            dicSlowDownRates = new Dictionary<int, float>();
            CurrentSlowRate = 1;
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
                    targetPlayer.Damage(EntityDataEnemy.EnemyData.Damage);
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
            hide = false;
            agent.enabled = true;
            agent.speed = EntityDataEnemy.EnemyData.Speed * CurrentSlowRate;

            levelPath = EntityDataEnemy.LevelPath;

            targetPathNodeIndex = 0;

            hp = EntityDataEnemy.EnemyData.MaxHP;

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
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

            hide = true;

            dataPlayer = null;

            RemoveSlowEffect();
            HideHpBar();
            dicSlowDownRates.Clear();
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

            if (!loadedHPBar)
            {
                GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                    (int)EnumEntity.HPBar,
                    typeof(EntityHPBar),
                    OnLoadHpBarSuccess,
                    EntityDataFollower.Create(hpBarRoot)));

                loadedHPBar = true;
            }

            if (entityHPBar)
            {
                entityHPBar.UpdateHealth(hp / EntityDataEnemy.EnemyData.MaxHP);
            }

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

            GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                EntityDataEnemy.EnemyData.DeadEffcetEntityId,
                typeof(EntityParticleAutoHide),
                null,
                EntityDataFollower.Create(randomSound.GetRandomSound(), transform.position + EntityDataEnemy.EnemyData.DeadEffectOffset, transform.rotation)));

            dataPlayer.AddEnergy(EntityDataEnemy.EnemyData.AddEnergy);
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
            if (hide)
                return;

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
            if (slowDownEffect == null && !loadSlowDownEffect)
            {
                GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create((int)EnumEntity.SlowFx,
                    typeof(EntityAnimation),
                    OnLoadSlowEffectSuccess,
                    EntityDataFollower.Create(transform,
                    EntityDataEnemy.EnemyData.ApplyEffectOffset,
                    Vector3.one * EntityDataEnemy.EnemyData.ApplyEffectScale,
                    EnumSound.None,
                    transform.position,
                    transform.rotation)
                    )
                    );

                loadSlowDownEffect = true;
            }
        }

        private void OnLoadSlowEffectSuccess(Entity entity)
        {
            slowDownEffect = entity;
            //若减速效果加载出后后，此敌人已经死亡或回收，则立马移除特效
            if (hide)
            {
                RemoveSlowEffect();
            }
        }

        private void OnLoadHpBarSuccess(Entity entity)
        {
            entityHPBar = entity.Logic as EntityHPBar;
            if (hide)
            {
                HideHpBar();
            }
            else
            {
                entityHPBar.UpdateHealth(hp / EntityDataEnemy.EnemyData.MaxHP);
            }
        }

        private void RemoveSlowEffect()
        {
            if (slowDownEffect != null)
            {
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(slowDownEffect.Id));
                slowDownEffect = null;
                loadSlowDownEffect = false;
            }
        }

        private void HideHpBar()
        {
            if (entityHPBar)
            {
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(entityHPBar.Id));
                loadedHPBar = false;
                entityHPBar = null;
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


