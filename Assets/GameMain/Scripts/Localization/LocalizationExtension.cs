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
    public static class LocalizationExtension
    {
        public static void LoadDictionary(this LocalizationComponent localizationComponent, string dictionaryName, bool fromBytes, object userData = null)
        {
            if (string.IsNullOrEmpty(dictionaryName))
            {
                Log.Warning("Dictionary name is invalid.");
                return;
            }

            localizationComponent.LoadDictionary(dictionaryName, AssetUtility.GetDictionaryAsset(dictionaryName, fromBytes), Constant.AssetPriority.DictionaryAsset, userData);
        }
    }
}
