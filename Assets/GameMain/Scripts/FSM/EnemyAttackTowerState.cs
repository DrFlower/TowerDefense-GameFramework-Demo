using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Flower.EntityEnemy>;

namespace Flower
{
    public class EnemyAttackTowerState : FsmState<EntityEnemy>, IReference
    {
        private EntityEnemy owner;
        protected EntityTargetable m_TargetTower;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            owner = procedureOwner.Owner;
            owner.Agent.isStopped = true;
            owner.Attacker.enabled = false;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsPause)
                return;

            owner.Attacker.OnUpdate(elapseSeconds, realElapseSeconds);

            if (!owner.isPathBlocked)
            {
                ChangeState<EnemyMoveState>(procedureOwner);
                return;
            }

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
                ChangeState<EnemyMoveState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            owner = null;
            m_TargetTower = null;
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

        public static EnemyAttackTowerState Create()
        {
            EnemyAttackTowerState state = ReferencePool.Acquire<EnemyAttackTowerState>();
            return state;
        }

        public void Clear()
        {
            owner = null;
            m_TargetTower = null;
        }
    }
}

