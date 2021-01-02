using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Flower
{
    public class ProcedureCheckResources : ProcedureBase
    {
        private bool m_CheckResourcesComplete = false;
        private bool m_NeedUpdateResources = false;
        private int m_UpdateResourceCount = 0;
        private long m_UpdateResourceTotalZipLength = 0L;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_CheckResourcesComplete = false;
            m_NeedUpdateResources = false;
            m_UpdateResourceCount = 0;
            m_UpdateResourceTotalZipLength = 0L;

            GameEntry.Resource.CheckResources(OnCheckResourcesComplete);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_CheckResourcesComplete)
            {
                return;
            }

            if (m_NeedUpdateResources)
            {
                procedureOwner.SetData<VarInt32>("UpdateResourceCount", m_UpdateResourceCount);
                ChangeState<ProcedureUpdateResources>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedurePreload>(procedureOwner);
            }
        }

        private void OnCheckResourcesComplete(int movedCount, int removedCount, int updateCount, long updateTotalLength, long updateTotalZipLength)
        {
            IResourceGroup resourceGroup = GameEntry.Resource.GetResourceGroup("0");
            if (resourceGroup == null)
            {
                Log.Error("has no resource group '{0}',", "0");
                return;
            }

            m_CheckResourcesComplete = true;
            m_NeedUpdateResources = !resourceGroup.Ready;
            m_UpdateResourceCount = resourceGroup.TotalCount - resourceGroup.ReadyCount;
            m_UpdateResourceTotalZipLength = resourceGroup.TotalZipLength;
            Log.Info("Check resources complete, '{0}' resources need to update,  unzip length is '{1}'.", m_UpdateResourceCount.ToString(), (resourceGroup.TotalLength - resourceGroup.ReadyLength).ToString());
        }
    }
}
