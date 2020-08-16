using System.Collections;
using System.Collections.Generic;
using GameFramework.Localization;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Flower
{
    public class ProcedureCheckResources : ProcedureBase
    {
        private bool complete = false;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {

            base.OnEnter(procedureOwner);
            complete = false;

            if (GameEntry.Base.EditorResourceMode)
            {
                ChangeState<ProcedurePreload>(procedureOwner);
            }
            else if (GameEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Package)
            {
                GameEntry.Resource.InitResources(OnInitResourceComplete);
            }
            else if (GameEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Updatable)
            {
                GameEntry.Resource.CheckResources(OnCheckResourcesComplete);
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (complete)
                ChangeState<ProcedurePreload>(procedureOwner);
        }


        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void OnInitResourceComplete()
        {
            complete = true;
        }

        private void OnCheckResourcesComplete(int movedCount, int removedCount, int updateCount, long updateTotalLength, long updateTotalZipLength)
        {

        }
    }
}

