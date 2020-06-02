
namespace GameFramework.Data
{
    public abstract class Data : IData
    {
        public virtual string Name
        {
            get
            {
                return this.GetType().ToString();
            }
        }

        public abstract void Init();

        public abstract void Preload();

        public abstract void Load();

        public abstract void Unload();

        public abstract void Shutdown();

    }
}

