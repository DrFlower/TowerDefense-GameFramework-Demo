using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Flower.EntityEnemy>;
using UnityEngine.AI;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class FlyingEnemyMoveState : FsmState<EntityEnemy>, IReference
    {
        ProcedureOwner m_procedureOwner;
        private EntityEnemy owner;
        private int targetPathNodeIndex = 0;

        public FlyingEnemyMoveState()
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
            owner.Agent.enabled = true;
            m_procedureOwner = procedureOwner;
            owner.Agent.SetDestination(owner.LevelPath.PathNodes[targetPathNodeIndex].position);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsPause)
                return;

            if (owner.TargetPlayer != null)
            {
                ChangeState<EnemyAttackHomeBaseState>(procedureOwner);
                return;
            }


            if (owner.LevelPath == null || owner.LevelPath.PathNodes == null || owner.LevelPath.PathNodes.Length == 0)
                return;


            if (owner.LevelPath.PathNodes.Length > targetPathNodeIndex && owner.isAtDestination)
            {
                if (owner.LevelPath.PathNodes.Length - 1 != targetPathNodeIndex)
                {
                    owner.Agent.SetDestination(owner.LevelPath.PathNodes[++targetPathNodeIndex].position);
                }
            }

            owner.Agent.speed = owner.EntityDataEnemy.EnemyData.Speed * owner.CurrentSlowRate;


            if (owner.isPathBlocked)
            {
                ChangeState<FlyingEnemyPushingThroughState>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.TargetPathNodeIndex, targetPathNodeIndex);
            owner = null;
        }


        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }


        public static FlyingEnemyMoveState Create()
        {
            FlyingEnemyMoveState state = ReferencePool.Acquire<FlyingEnemyMoveState>();
            return state;
        }

        public void Clear()
        {
            targetPathNodeIndex = 0;
            m_procedureOwner.RemoveData(Constant.ProcedureData.TargetPathNodeIndex);
            owner = null;
        }
    }
}

