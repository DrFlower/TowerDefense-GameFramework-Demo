using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Flower.Editor.DataTableTools;
using GameFramework;
using System.Text;
using System.IO;
using System;

using Object = UnityEngine.Object;

namespace Flower
{
    public static class ResourceCollectionEx
    {
        //private static readonly string AssetConfigPath = "Assets/GameMain/DataTables/AssetsPath";
        private static readonly string AssetConfigName = "AssetsPath";
        private readonly static string EnumTemplateFileName = "Assets/GameMain/Scripts/Editor/ResourceCollectionEx/ResourceCollectionTemplate.json";
        private readonly static string GeneratePath = "Assets/GameMain/Scripts/Editor/ResourceCollectionEx";

        [MenuItem("Tools/GenerateResourceCollection", false, 7)]
        public static void GenerateResourceCollection()
        {
            DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(AssetConfigName);
            if (!DataTableGenerator.CheckRawData(dataTableProcessor, AssetConfigName))
            {
                Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", AssetConfigName));
                return;
            }

            GenerateEnumFile(dataTableProcessor, AssetConfigName);
            AssetDatabase.Refresh();
        }

        public static void GenerateEnumFile(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            dataTableProcessor.SetCodeTemplate(EnumTemplateFileName, Encoding.UTF8);
            dataTableProcessor.SetCodeGenerator(DataTableCodeGenerator);

            string csharpCodeFileName = Utility.Path.GetRegularPath(Path.Combine(GeneratePath, dataTableName + ".json"));
            if (!dataTableProcessor.GenerateCodeFile(csharpCodeFileName, Encoding.UTF8, dataTableName) && File.Exists(csharpCodeFileName))
            {
                File.Delete(csharpCodeFileName);
            }
        }

        private static void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
        {
            string dataTableName = (string)userData;
            codeContent.Replace("__RESOURCE_COLLECTION_ITEM__", GenerateEnumItems(dataTableProcessor));
        }


        private static string GenerateEnumItems(DataTableProcessor dataTableProcessor)
        {
            StringBuilder stringBuilder = new StringBuilder();

            int startRow = 4;
            dicMainAsset.Clear();
            dicDependency.Clear();
            for (int i = startRow; i < dataTableProcessor.RawRowCount; i++)
            {
                int index = i - startRow;

                if (dataTableProcessor.GetValue(i, 0).StartsWith("#"))
                    continue;

                int id = int.Parse(dataTableProcessor.GetValue(i, 1));
                string path = dataTableProcessor.GetValue(i, 3);
                string name = Path.GetFileNameWithoutExtension(path);
                string resourceGroupsStr = dataTableProcessor.GetValue(i, 4);


                string[] resourceGroups = resourceGroupsStr.Split('|');
                bool pack = false;


                for (int j = 0; j < resourceGroups.Length; j++)
                {
                    if (resourceGroups[j] == "0")
                        pack = true;
                }

                MainAsset mainAsset = new MainAsset(name, path, null, null, 0, pack, resourceGroups);
                dicMainAsset.Add(path, mainAsset);


                List<string> deps = GetDenpendencies(path);

                foreach (var item in deps)
                {
                    Dependency dep = null;
                    if (!dicDependency.TryGetValue(item, out dep))
                    {
                        dep = new Dependency(item);
                        dicDependency.Add(item, dep);
                    }

                    dep.AddDenpendency(path, mainAsset);
                }



                stringBuilder.AppendFormat("\"{0}\": ", id).AppendLine();
                stringBuilder.Append("{").AppendLine();
                stringBuilder.AppendFormat("\"Name\": \"{0}\",", name).AppendLine();
                stringBuilder.AppendFormat("\"Path\": \"{0}\",", path).AppendLine();
                stringBuilder.AppendFormat("\"Variant\": \"{0}\",", "").AppendLine();
                stringBuilder.AppendFormat("\"FileSystem\": \"{0}\",", "").AppendLine();
                stringBuilder.AppendFormat("\"LoadType\": \"{0}\",", 0).AppendLine();
                stringBuilder.AppendFormat("\"Packed\": \"{0}\",", "False").AppendLine();
                stringBuilder.AppendFormat("\"ResourceGroups\": \"{0}\"", "1,2,3,4,5").AppendLine();
                stringBuilder.Append("},").AppendLine();
            }

            List<Dependency> list = new List<Dependency>();
            int count = 0;
            foreach (var item in dicDependency)
            {
                list.Add(item.Value);
                if (item.Value.dependCount > 1)
                    count++;
            }

            list.Sort((a, b) =>
            {
                if (a.dependCount < b.dependCount)
                {
                    return 1;
                }
                else if (a.dependCount == b.dependCount)
                {
                    return 0;
                }
                return -1;
            }
            );
            //Debug.LogError(count);
            string str = LitJson.JsonMapper.ToJson(dicMainAsset);
            File.WriteAllText("Assets/GameMain/Scripts/Editor/ResourceCollectionEx/MainAsset.json", str);

            string str2 = LitJson.JsonMapper.ToJson(dicDependency);
            File.WriteAllText("Assets/GameMain/Scripts/Editor/ResourceCollectionEx/Dependency.json", str2);
            return stringBuilder.ToString();
        }

        private static void MoveFile(List<Dependency> list)
        {
            foreach (var item in list)
            {
                MoveAsset(item.path);
            }

            AssetDatabase.Refresh();
        }

        private static void MoveAsset(string assetPath)
        {
            string sourceAssetPathPrefix = "Assets/GameAssets/";

            if (!assetPath.StartsWith(sourceAssetPathPrefix))
            {
                Debug.LogError(string.Format("Path prefix error:{0}", sourceAssetPathPrefix));
                return;
            }

            string targetPath = "Assets/GameMain/Res/" + assetPath.Substring(sourceAssetPathPrefix.Length);

            string targetDir = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
                AssetDatabase.Refresh();
            }

            string s = AssetDatabase.MoveAsset(assetPath, targetPath);
            if (string.IsNullOrEmpty(s))
            {
                Debug.LogError(string.Format("asset '{0}' move to '{1}'", assetPath, targetPath));
            }
            else
            {
                Debug.LogError(s);
            }
        }

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
                if (resourceGroup == null)
                {
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

                }

                return resourceGroup;
            }

            public bool IsPacked()
            {
                if (resourceGroup == null)
                {
                    GetResourceGroups();
                }

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
                foreach (var groups in item.Value.GetResourceGroups())
                {
                    str += groups + ",";
                }
            }

        }

        private static List<string> GetDenpendencies(string assetPath)
        {

            var deps = AssetDatabase.GetDependencies(assetPath, false);

            List<string> list = new List<string>();



            foreach (var dep in deps)
            {
                if (dep.EndsWith(".cs") || dep == assetPath || dep.EndsWith(".unity"))
                    continue;

                if (dep.StartsWith("Assets/GameAssets/"))
                    Debug.LogError(dep + ":   " + assetPath);

                GetDenpendencies(dep, list);


                list.Add(dep);
            }

            list.Sort(EditorUtility.NaturalCompare);

            return list;
        }
        private static void GetDenpendencies(string assetPath, List<string> list)
        {
            var deps = AssetDatabase.GetDependencies(assetPath, false);
            foreach (var dep in deps)
            {
                if (dep.EndsWith(".cs") || dep == assetPath || dep.EndsWith(".unity"))
                    continue;

                list.Add(dep);
            }
        }
    }
}
