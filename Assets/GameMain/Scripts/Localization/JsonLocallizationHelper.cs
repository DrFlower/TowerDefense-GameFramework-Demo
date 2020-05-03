using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using LitJson;

namespace Flower
{
    /// <summary>
    /// XML 格式的本地化辅助器。
    /// </summary>
    public class JsonLocallizationHelper : DefaultLocalizationHelper
    {
        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="dictionaryData">要解析的字典数据。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public override bool ParseDictionary(object dictionaryData, object userData)
        {
            try
            {
                string dictionaryText = dictionaryData as string;
                if (dictionaryText != null)
                {
                    string currentLanguage = GameEntry.Localization.Language.ToString();


                    List<LocalizationSerializableObject> localizationSerializableObjects = JsonMapper.ToObject<List<LocalizationSerializableObject>>(dictionaryText);

                    foreach (var localizationSerializableObject in localizationSerializableObjects)
                    {
                        if (localizationSerializableObject.language != currentLanguage)
                        {
                            continue;
                        }

                        foreach (var item in localizationSerializableObject.dic)
                        {
                            if (!AddRawString(item.Key, item.Value))
                            {
                                Log.Warning("Can not add raw string with key '{0}' which may be invalid or duplicate.", item.Key);
                                return false;
                            }
                        }
                    }
                    return true;
                }

                Log.Warning("Can not parse dictionary data which type '{0}' is invalid.", dictionaryData.GetType().FullName);
                return false;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse dictionary data with exception '{0}'.", exception.ToString());
                return false;
            }
        }
    }
}



