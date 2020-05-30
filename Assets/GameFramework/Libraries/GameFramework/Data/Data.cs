
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

        public abstract void OnPreload();

        public abstract void OnLoad();

        public abstract void OnUnload();

        public abstract void Shutdown();

    }
}

