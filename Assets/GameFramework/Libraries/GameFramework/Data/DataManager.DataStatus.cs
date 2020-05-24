
namespace GameFramework.Data
{
    internal sealed partial class DataManager : GameFrameworkModule, IDataManager
    {
        public enum DataStatus
        {
            None,
            Inited,
            Preloaded,
            Loaded,
            Unloaded,
        }
    }
}

