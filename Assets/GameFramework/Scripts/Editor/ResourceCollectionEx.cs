using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameFramework;
using System.Text;
using System.IO;
using System;

using Object = UnityEngine.Object;

namespace UnityGameFramework
{
    public static class ResourceCollectionEx
    {
        public static bool GetResroucePack(string resourceName)
        {
            if (dicMainAsset.ContainsKey(resourceName))
            {
                return dicMainAsset[resourceName].Packed;
            }

            if (dicDependency.ContainsKey(resourceName))
            {
                return dicDependency[resourceName].IsPacked();
            }

            Debug.LogError("Can't get pack:" + resourceName);

            return false;
        }

        public static string[] GetResourceGroups(string resourceName)
        {
            if (dicMainAsset.ContainsKey(resourceName))
            {
                return dicMainAsset[resourceName].ResourceGroups;
            }

            if (dicDependency.ContainsKey(resourceName))
            {
                return dicDependency[resourceName].GetResourceGroups();
            }

            Debug.LogError("Can't get resrouce group:" + resourceName);

            return new string[0];
        }

        private static Dictionary<string, MainAsset> dicMainAsset = new Dictionary<string, MainAsset>();
        private static Dictionary<string, Dependency> dicDependency = new Dictionary<string, Dependency>();

        public class MainAsset
        {
            public string name;
            public string Path;
            public string Variant;
            public string FileSystem;
            public int LoadType;
            public bool Packed;
            public string[] ResourceGroups;

            public MainAsset()
            {

            }

            public MainAsset(string name, string path, string variant, string fileSystem, int loadType, bool packed, string[] resourceGroups)
            {
                this.name = name;
                Path = path;
                Variant = variant;
                FileSystem = fileSystem;
                LoadType = loadType;
                Packed = packed;
                ResourceGroups = resourceGroups;
            }
        }

        public class Dependency
        {
            public string name;
            public string path;
            public int dependCount;
            public Dictionary<string, MainAsset> AssetDependToThis = new Dictionary<string, MainAsset>();
            private List<string> resourceGroupList = new List<string>();
            private string[] resourceGroup;

            public Dependency()
            {

            }

            public void Process()
            {
                foreach (var item in AssetDependToThis)
                {
                    dependCount++;

                    foreach (var group in item.Value.ResourceGroups)
                    {
                        if (!resourceGroupList.Contains(group))
                            resourceGroupList.Add(group);
                    }
                }

                resourceGroupList.Sort((a, b) =>
                {
                    if (int.Parse(a) > int.Parse(b))
                    {
                        return 1;
                    }
                    else if (int.Parse(a) == int.Parse(b))
                    {
                        return 0;
                    }
                    return -1;
                });
                resourceGroup = new string[resourceGroupList.Count];

                for (int i = 0; i < resourceGroupList.Count; i++)
                {
                    resourceGroup[i] = resourceGroupList[i];
                }

                if (resourceGroup[0] == "0")
                {
                    resourceGroup = new string[1] { "0" };
                }

            }

            public Dependency(string path)
            {
                this.path = path;
                this.name = Path.GetFileNameWithoutExtension(path);
            }

            public void AddDenpendency(string assetName, MainAsset mainAsset)
            {
                if (AssetDependToThis.ContainsKey(assetName))
                    return;

                AssetDependToThis.Add(assetName, mainAsset);
                dependCount++;

                foreach (var item in mainAsset.ResourceGroups)
                {
                    if (!resourceGroupList.Contains(item))
                        resourceGroupList.Add(item);
                }
            }

            public string[] GetResourceGroups()
            {
                //if (resourceGroup == null)
                //{
                //    resourceGroupList.Sort((a, b) =>
                //    {
                //        if (int.Parse(a) > int.Parse(b))
                //        {
                //            return 1;
                //        }
                //        else if (int.Parse(a) == int.Parse(b))
                //        {
                //            return 0;
                //        }
                //        return -1;
                //    });
                //    resourceGroup = new string[resourceGroupList.Count];

                //    for (int i = 0; i < resourceGroupList.Count; i++)
                //    {
                //        resourceGroup[i] = resourceGroupList[i];
                //    }

                //}
                return resourceGroup;
            }

            public bool IsPacked()
            {
                //if (resourceGroup == null)
                //{
                //    GetResourceGroups();
                //}

                if (resourceGroup.Length > 0 && resourceGroup[0] == "0")
                    return true;

                return false;
            }
        }

        public static void InitInfo()
        {
            string jsonPath1 = "Assets/GameMain/Scripts/Editor/ResourceCollectionEx/MainAsset.json";
            string json1 = File.ReadAllText(jsonPath1);
            dicMainAsset = LitJson.JsonMapper.ToObject<Dictionary<string, MainAsset>>(json1);

            string jsonPath2 = "Assets/GameMain/Scripts/Editor/ResourceCollectionEx/Dependency.json";
            string json2 = File.ReadAllText(jsonPath2);
            dicDependency = LitJson.JsonMapper.ToObject<Dictionary<string, Dependency>>(json2);



            foreach (var item in dicDependency)
            {
                string str = string.Empty;
                item.Value.Process();
            }

        }
    }
}
