namespace Flower.Data
{

    public sealed class ItemGroupData
    {
        private DRItemGroup dRItemGroup;
        private PoolParamData poolParamData;

        public int Id
        {
            get
            {
                return dRItemGroup.Id;
            }
        }

        public string Name
        {
            get
            {
                return dRItemGroup.Name;
            }
        }

        public PoolParamData PoolParamData
        {
            get
            {
                return poolParamData;
            }
        }

        public ItemGroupData(DRItemGroup dRItemGroup, PoolParamData poolParamData)
        {
            this.dRItemGroup = dRItemGroup;
            this.poolParamData = poolParamData;
        }
    }
}
