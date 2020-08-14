using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Flower.Data;
using UnityGameFramework.Runtime;
using System;
using GameFramework.Fsm;
using GameFramework;

namespace Flower
{
    public class EntityEnemy : EntityTargetable, IPause
    {
        public Transform turret;
        public Transform[] projectilePoints;
        public Transform epicenter;
        public Launcher launcher;

        protected IFsm<EntityEnemy> fsm;

        private DataPlayer dataPlayer;

        private Dictionary<int, float> dicSlowDownRates;

        //表示是否死亡或已攻击玩家即将回收，以防重复执行回收逻辑
        private bool hide = false;

        private Entity slowDownEffect;
        private bool loadSlowDownEffect = false;

        protected List<FsmState<EntityEnemy>> stateList;

        public Targetter Targetter
        {
            get;
            private set;
        }

        public Attacker Attacker
        {
            get;
            private set;
        }

        public override EnumAlignment Alignment
        {
            get
            {
                return EnumAlignment.Enemy;
            }
        }

        protected override float MaxHP
        {
            get
            {
                if (EntityDataEnemy != null)
                    return EntityDataEnemy.EnemyData.MaxHP;
                else
                    return 0;
            }
        }

        public EntityDataEnemy EntityDataEnemy
        {
            get;
            private set;
        }

        public float CurrentSlowRate
        {
            get;
            private set;
        }

        public NavMeshAgent Agent
        {
            get;
            private set;
        }

        public bool isPathBlocked
        {
            get { return Agent.pathStatus == NavMeshPathStatus.PathPartial; }
        }

        public bool isAtDestination
        {
            get { return Agent.remainingDistance <= Agent.stoppingDistance; }
        }

        public LevelPath LevelPath
        {
            get;
            private set;
        }

        public EntityPlayer TargetPlayer
        {
            get;
            private set;
        }

        public bool IsPause
        {
            get;
            private set;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            Agent = GetComponent<NavMeshAgent>();
            hpBarRoot = transform.Find("HealthBar");
            dicSlowDownRates = new Dictionary<int, float>();
            stateList = new List<FsmState<EntityEnemy>>();
            CurrentSlowRate = 1;

            Targetter = transform.Find("Targetter").GetComponent<Targetter>();
            Attacker = transform.Find("Attack").GetComponent<Attacker>();

            Targetter.OnInit(userData);
            Attacker.OnInit(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (IsPause)
                return;
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
            Agent.enabled = true;
            LevelPath = EntityDataEnemy.LevelPath;
            hp = EntityDataEnemy.EnemyData.MaxHP;
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            Attacker.SetOwnerEntity(Entity);
            Targetter.SetAlignment(Alignment);
            Targetter.SetTurret(turret);
            Targetter.SetSearchRange(EntityDataEnemy.EnemyData.Range);
            Targetter.ResetTargetter();

            AttackerData attackerData = AttackerData.Create(EntityDataEnemy.EnemyData.Range,
                EntityDataEnemy.EnemyData.FireRate,
                EntityDataEnemy.EnemyData.IsMultiAttack,
                EntityDataEnemy.EnemyData.ProjectileType,
                EntityDataEnemy.EnemyData.ProjectileEntityId
                );

            Attacker.SetData(attackerData, EntityDataEnemy.EnemyData.ProjectileData);
            Attacker.SetTargetter(Targetter);
            Attacker.SetProjectilePoints(projectilePoints);
            Attacker.SetEpicenter(epicenter);
            Attacker.SetLaunch(launcher);
            Attacker.ResetAttack();

            Targetter.OnShow(userData);
            Attacker.OnShow(userData);

            CreateFsm();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            Targetter.OnHide(isShutdown, userData);
            Attacker.OnHide(isShutdown, userData);
            Attacker.EmptyOwnerEntity();

            LevelPath = null;
            EntityDataEnemy = null;
            hp = 0;
            Agent.enabled = false;
            TargetPlayer = null;
            hide = true;
            dataPlayer = null;
            DestroyFsm();
            RemoveSlowEffect();
            dicSlowDownRates.Clear();
        }

        protected virtual void AddFsmState()
        {
            stateList.Add(EnemyMoveState.Create());
            stateList.Add(EnemyAttackHomeBaseState.Create());
            stateList.Add(EnemyAttackTowerState.Create());
        }

        protected virtual void StartFsm()
        {
            fsm.Start<EnemyMoveState>();
        }

        private void CreateFsm()
        {
            AddFsmState();
            fsm = GameEntry.Fsm.CreateFsm<EntityEnemy>(gameObject.name, this, stateList);
            StartFsm();
        }

        private void DestroyFsm()
        {
            GameEntry.Fsm.DestroyFsm(fsm);
            foreach (var item in stateList)
            {
                ReferencePool.Release((IReference)item);
            }

            stateList.Clear();
            fsm = null;
        }

        public void AfterAttack()
        {
            if (!hide)
            {
                hide = true;
                GameEntry.Event.Fire(this, HideEnemyEventArgs.Create(Id));
            }
        }

        protected override void Dead()
        {
            base.Dead();

            dataPlayer.AddEnergy(EntityDataEnemy.EnemyData.AddEnergy);
            if (!hide)
            {
                hide = true;
                GameEntry.Event.Fire(this, HideEnemyEventArgs.Create(Id));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<EntityPlayer>();
            if (player == null)
            {
                return;
            }
            TargetPlayer = player;
        }

        public void ApplySlow(int towerId, float slowRate)
        {
            if (IsDead || !Available)
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
        }

        private void ApplySlowEffect()
        {
            if (slowDownEffect == null && !loadSlowDownEffect)
            {
                GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create((int)EnumEntity.SlowFx,
                    typeof(EntityAnimation),
                    OnLoadSlowEffectSuccess,
                    EntityDataFollower.Create(transform,
                    ApplyEffectOffset,
                    Vector3.one * ApplyEffectScale,
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

        private void RemoveSlowEffect()
        {
            if (slowDownEffect != null)
            {
                GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(slowDownEffect.Id));
                slowDownEffect = null;
                loadSlowDownEffect = false;
            }
        }

        public void Pause()
        {
            IsPause = true;
            Agent.speed = 0;
        }

        public void Resume()
        {
            IsPause = false;
            Agent.speed = EntityDataEnemy.EnemyData.Speed * CurrentSlowRate;
        }
    }
}