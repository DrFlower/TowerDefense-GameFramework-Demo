
namespace Flower.Data
{
    public sealed class EntityGroupData
    {
        private DREntityGroup dREntityGroup;
        private PoolParamData poolParamData;

        public int Id
        {
            get
            {
                return dREntityGroup.Id;
            }
        }

        public string Name
        {
            get
            {
                return dREntityGroup.Name;
            }
        }

        public PoolParamData PoolParamData
        {
            get
            {
                return poolParamData;
            }
        }

        public EntityGroupData(DREntityGroup dREntityGroup, PoolParamData poolParamData)
        {
            this.dREntityGroup = dREntityGroup;
            this.poolParamData = poolParamData;
        }
    }
}
