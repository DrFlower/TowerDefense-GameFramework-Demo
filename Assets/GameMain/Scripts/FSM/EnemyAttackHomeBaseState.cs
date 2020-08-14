using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Flower.EntityEnemy>;

namespace Flower
{
    public class EnemyAttackHomeBaseState : FsmState<EntityEnemy>, IReference
    {
        private EntityEnemy owner;
        private bool attacked = false;
        private float attackTimer = 0;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            owner = procedureOwner.Owner;

            if (owner.TargetPlayer != null)
                owner.TargetPlayer.Charge();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsPause)
                return;

            if (owner.TargetPlayer != null)
            {
                if (owner.TargetPlayer != null && !attacked)
                {
                    attackTimer += elapseSeconds;
                    if (attackTimer > 1)
                    {
                        owner.TargetPlayer.Damage(owner.EntityDataEnemy.EnemyData.Damage);
                        attacked = true;
                        owner.AfterAttack();
                    }
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }


        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        public static EnemyAttackHomeBaseState Create()
        {
            EnemyAttackHomeBaseState state = ReferencePool.Acquire<EnemyAttackHomeBaseState>();
            return state;
        }

        public void Clear()
        {
            owner = null;
            attacked = false;
            attackTimer = 0;
        }
    }
}

