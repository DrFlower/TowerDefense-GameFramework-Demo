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
        private int targetPathNodeIndex = 0;

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

            targetPathNodeIndex = 0;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            var owner = procedureOwner.Owner;

            if (owner.IsPause)
                return;

            if (owner.TargetPlayer != null)
            {
                ChangeState<EnemyAttackHomeBaseState>(procedureOwner);
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


            //owner.Targetter.transform.position = owner.Agent.pathEndPosition;
            //EntityTargetable tower = owner.Targetter.GetTarget();
            //if (tower != m_TargetTower)
            //{
            //    // if the current target is to be replaced, unsubscribe from removed event
            //    if (m_TargetTower != null)
            //    {
            //        m_TargetTower.removed -= OnTargetTowerDestroyed;
            //    }

            //    // assign target, can be null
            //    m_TargetTower = tower;

            //    // if new target found subscribe to removed event
            //    if (m_TargetTower != null)
            //    {
            //        m_TargetTower.removed += OnTargetTowerDestroyed;
            //    }
            //}
            //if (m_TargetTower == null)
            //{
            //    return;
            //}
            //float distanceToTower = Vector3.Distance(transform.position, m_TargetTower.transform.position);
            //if (!(distanceToTower < m_AttackAffector.towerTargetter.effectRadius))
            //{
            //    return;
            //}
            //if (!m_AttackAffector.enabled)
            //{
            //    m_AttackAffector.towerTargetter.transform.position = transform.position;
            //    m_AttackAffector.enabled = true;
            //}
            //state = State.Attacking;
            //m_NavMeshAgent.isStopped = true;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }


        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
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

