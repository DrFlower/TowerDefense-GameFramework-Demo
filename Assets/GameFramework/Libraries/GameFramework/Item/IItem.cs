namespace GameFramework.Item
{
    public interface IItem
    {
        int Id
        {
            get;
        }

        string ItemAssetName
        {
            get;
        }

        object Handle
        {
            get;
        }

        IItemGroup ItemGroup
        {
            get;
        }

        void OnInit(int itemId, string itemAssetName, IItemGroup itemGroup, bool isNewInstance, object userData);

        void OnRecycle();

        void OnShow(object userData);

        void OnHide(bool isShutdown, object userData);

        void OnUpdate(float elapseSeconds, float realElapseSeconds);
    }
}

