namespace GameFramework.Data
{
    public interface IData
    {
        string Name { get; }

        void Init();

        void OnPreload();

        void OnLoad();

        void OnUnload();

        void Shutdown();
    }
}

