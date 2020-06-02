namespace GameFramework.Data
{
    public interface IData
    {
        string Name { get; }

        void Init();

        void Preload();

        void Load();

        void Unload();

        void Shutdown();
    }
}

