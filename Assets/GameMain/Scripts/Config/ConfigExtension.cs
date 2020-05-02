//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityGameFramework.Runtime;

namespace Flower
{
    public static class ConfigExtension
    {
        public static void LoadConfig(this ConfigComponent configComponent, string configName, bool fromBytes, object userData = null)
        {
            if (string.IsNullOrEmpty(configName))
            {
                Log.Warning("Config name is invalid.");
                return;
            }

            configComponent.LoadConfig(configName, AssetUtility.GetConfigAsset(configName, fromBytes), Constant.AssetPriority.ConfigAsset, userData);
        }
    }
}
