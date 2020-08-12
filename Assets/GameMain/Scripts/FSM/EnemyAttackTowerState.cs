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
                ChangeState<EnemyMoveState>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            owner = null;
        }


        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        public static EnemyAttackTowerState Create()
        {
            EnemyAttackTowerState state = ReferencePool.Acquire<EnemyAttackTowerState>();
            return state;
        }

        public void Clear()
        {

        }
    }
}

