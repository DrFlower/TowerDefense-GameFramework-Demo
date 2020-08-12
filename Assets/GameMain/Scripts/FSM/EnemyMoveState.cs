using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Flower.EntityEnemy>;

namespace Flower
{
    public class EnemyMoveState : FsmState<EntityEnemy>, IReference
    {
        private EntityEnemy owner;
        private int targetPathNodeIndex = 0;
        protected EntityTargetable m_TargetTower;

        public EnemyMoveState()
        {

        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            owner = procedureOwner.Owner;
            owner.Agent.isStopped = false;
            owner.Attacker.enabled = false;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsPause)
                return;

            owner.Targetter.OnUpdate(elapseSeconds, realElapseSeconds);

            if (owner.TargetPlayer != null)
            {
                ChangeState<EnemyAttackHomeBaseState>(procedureOwner);
                return;
            }

            if (owner.LevelPath == null || owner.LevelPath.PathNodes == null || owner.LevelPath.PathNodes.Length == 0)
                return;

            if (owner.LevelPath.PathNodes.Length > targetPathNodeIndex)
            {
                owner.Agent.SetDestination(owner.LevelPath.PathNodes[targetPathNodeIndex].position);

                if (owner.isAtDestination)
                {
                    if (owner.LevelPath.PathNodes.Length - 1 == targetPathNodeIndex)
                    {
                        //owner.Agent.isStopped = true;
                    }
                    else
                    {
                        targetPathNodeIndex++;
                    }
                }
            }

            owner.Agent.speed = owner.EntityDataEnemy.EnemyData.Speed * owner.CurrentSlowRate;

            if (owner.isPathBlocked)
            {
                owner.Targetter.transform.position = owner.Agent.pathEndPosition;
                EntityTargetable tower = owner.Targetter.GetTarget();
                if (tower != m_TargetTower)
                {
                    // if the current target is to be replaced, unsubscribe from removed event
                    if (m_TargetTower != null)
                    {
                        m_TargetTower.OnHidden -= OnTargetTowerDestroyed;
                    }

                    // assign target, can be null
                    m_TargetTower = tower;

                    // if new target found subscribe to removed event
                    if (m_TargetTower != null)
                    {
                        m_TargetTower.OnHidden += OnTargetTowerDestroyed;
                    }
                }
                if (m_TargetTower == null)
                {
                    return;
                }
                float distanceToTower = Vector3.Distance(owner.transform.position, m_TargetTower.transform.position);
                if (distanceToTower > owner.EntityDataEnemy.EnemyData.Range)
                {
                    return;
                }

                ChangeState<EnemyAttackTowerState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            owner.Targetter.transform.position = owner.transform.position;
            owner = null;
        }


        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void OnTargetTowerDestroyed(EntityTargetable target)
        {
            if (m_TargetTower == target)
            {
                m_TargetTower.OnHidden -= OnTargetTowerDestroyed;
                m_TargetTower = null;
            }
        }

        public static EnemyMoveState Create()
        {
            EnemyMoveState state = ReferencePool.Acquire<EnemyMoveState>();
            return state;
        }

        public void Clear()
        {
            targetPathNodeIndex = 0;
        }
    }
}

