namespace Flower.Data
{
    public sealed class ItemData
    {
        private DRItem dRItem;
        private DRAssetsPath dRAssetsPath;
        private ItemGroupData itemGroupData;

        public int Id
        {
            get
            {
                return dRItem.Id;
            }
        }
        public string Name
        {
            get
            {
                return dRItem.Name;
            }
        }

        public string AssetPath
        {
            get
            {
                return dRAssetsPath.AssetPath;
            }
        }

        public ItemGroupData ItemGroupData
        {
            get
            {
                return itemGroupData;
            }
        }

        public ItemData(DRItem dRItem, DRAssetsPath dRAssetsPath, ItemGroupData itemGroupData)
        {
            this.dRItem = dRItem;
            this.dRAssetsPath = dRAssetsPath;
            this.itemGroupData = itemGroupData;
        }

    }
}