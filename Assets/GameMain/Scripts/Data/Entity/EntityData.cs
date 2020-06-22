
namespace Flower.Data
{
    public sealed class EntityData
    {
        private DREntity dREntity;
        private DRAssetsPath dRAssetsPath;
        private EntityGroupData entityGroupData;

        public int Id
        {
            get
            {
                return dREntity.Id;
            }
        }
        public string Name
        {
            get
            {
                return dREntity.Name;
            }
        }

        public string AssetPath
        {
            get
            {
                return dRAssetsPath.AssetPath;
            }
        }

        public EntityGroupData EntityGroupData
        {
            get
            {
                return entityGroupData;
            }
        }

        public EntityData(DREntity dREntity, DRAssetsPath dRAssetsPath, EntityGroupData entityGroupData)
        {
            this.dREntity = dREntity;
            this.dRAssetsPath = dRAssetsPath;
            this.entityGroupData = entityGroupData;
        }

    }
}

