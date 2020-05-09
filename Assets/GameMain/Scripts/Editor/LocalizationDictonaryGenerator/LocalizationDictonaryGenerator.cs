using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.UI;
using LitJson;

namespace Flower
{
    public static class LocalizationDictonaryGenerator
    {
        /// <summary>
        /// Prefab路径
        /// </summary>
        private static readonly string PrefabRootPath = @"Assets/GameMain/UI/UIForms";
        private static readonly string GeneratePathFormat = Application.dataPath + "/GameMain/Localization/{0}/Dictionaries";
        private static readonly string GenerateLanguage = "English";
        private static readonly string GenerateFileName = "Default";
        private static readonly string SuffixName = "json";

        public static void GenerateEnglishLocalizationDictionary()
        {
            try
            {
                string[] prefabPaths = GetAllPrefabs(PrefabRootPath);

                GameObject[] prefabs = new GameObject[prefabPaths.Length];
                for (int i = 0; i < prefabs.Length; i++)
                {
                    prefabs[i] = LoadPrefabFromPath(prefabPaths[i]);
                }

                var dic = GetLocalizationDictionaryFromPrefabs(prefabs);

                LocalizationSerializableObject obj = new LocalizationSerializableObject();
                obj.language = GenerateLanguage;
                obj.dic = dic;

                List<LocalizationSerializableObject> list = new List<LocalizationSerializableObject>() { obj };

                string json = JsonMapper.ToJson(list);

                string path = string.Format(GeneratePathFormat, GenerateLanguage);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = string.Format("{0}.{1}", GenerateFileName, SuffixName);
                string fullPath = path + "/" + fileName;

                File.WriteAllText(fullPath, json);
                AssetDatabase.Refresh();
                Debug.Log("Generate English Localization Dictionay Success! Path:" + fullPath);
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        private static Dictionary<string, string> GetLocalizationDictionaryFromPrefabs(GameObject[] prefabs)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var prefab in prefabs)
            {
                Text[] texts = prefab.GetComponentsInChildren<Text>(true);
                foreach (var text in texts)
                {
                    dic.Add(text.text, text.text);
                }
            }

            return dic;
        }

        private static GameObject LoadPrefabFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

            return go;
        }

        private static string[] GetAllPrefabs(string directory)
        {
            if (string.IsNullOrEmpty(directory) || !directory.StartsWith("Assets"))
                throw new ArgumentException("folderPath error");

            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { directory });
            string[] assetPaths = new string[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                assetPaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            }

            return assetPaths;
        }
    }
}


