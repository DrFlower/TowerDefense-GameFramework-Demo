
namespace GameFramework.Data
{
    public abstract class DataBase : IData
    {
        public abstract string Name { get; }

        public abstract void Init();

        public abstract void OnPreload();

        public abstract void OnLoad();

        public abstract void OnUnload();

        public abstract void Shutdown();

    }
}

