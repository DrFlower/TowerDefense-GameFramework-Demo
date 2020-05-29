using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        //public static BuiltinDataComponent BuiltinData
        //{
        //    get;
        //    private set;
        //}

        public static ItemComponent ItemComponent
        {
            get;
            private set;
        }

        public static DataComponent DataComponent
        {
            get;
            private set;
        }


        private static void InitCustomComponents()
        {
            ItemComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<ItemComponent>();
            DataComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<DataComponent>();
        }
    }
}
