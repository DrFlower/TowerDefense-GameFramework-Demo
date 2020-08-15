using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Flower.Editor.DataTableTools;
using System.Text;
using System;
using UnityEditor;
using GameFramework;

namespace Flower
{
    public static class DataTableEnumGenerator
    {
        private readonly static string EnumTemplateFileName = "Assets/GameMain/Configs/EnumTemplate.txt";
        private readonly static string GeneratePath = "Assets/GameMain/Scripts/Enum";

        private readonly static string[] GenerateDataTables = {
            "UIForm",
            "Entity",
            "Item",
            "Sound",
        };

        [MenuItem("Tools/Generate DataTable Enum", false, 3)]
        private static void GenerateDataTableEnum()
        {
            foreach (string dataTableName in GenerateDataTables)
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                GenerateEnumFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }

        public static void GenerateEnumFile(DataTableProcessor dataTableProcessor, string dataTableName)
        {
            dataTableProcessor.SetCodeTemplate(EnumTemplateFileName, Encoding.UTF8);
            dataTableProcessor.SetCodeGenerator(DataTableCodeGenerator);

            string csharpCodeFileName = Utility.Path.GetRegularPath(Path.Combine(GeneratePath, "Enum" + dataTableName + ".cs"));
            if (!dataTableProcessor.GenerateCodeFile(csharpCodeFileName, Encoding.UTF8, dataTableName) && File.Exists(csharpCodeFileName))
            {
                File.Delete(csharpCodeFileName);
            }
        }

        private static void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
        {
            string dataTableName = (string)userData;

            codeContent.Replace("__DATA_TABLE_CREATE_TIME__", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            codeContent.Replace("__DATA_TABLE_NAME_SPACE__", "Flower");
            codeContent.Replace("__DATA_TABLE_ENUM_NAME__", "Enum" + dataTableName);
            //codeContent.Replace("__DATA_TABLE_COMMENT__", dataTableProcessor.GetValue(0, 1) + "。");
            codeContent.Replace("__DATA_TABLE_ENUM_ITEM__", GenerateEnumItems(dataTableProcessor));
        }

        private static string GenerateEnumItems(DataTableProcessor dataTableProcessor)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool firstProperty = true;

            int startRow = 4;

            stringBuilder
             .AppendLine("        /// <summary>")
             .AppendFormat("        /// {0}", "无").AppendLine()
             .AppendLine("        /// </summary>")
             .AppendFormat("        {0} = {1},", "None", "0").AppendLine().AppendLine();

            for (int i = startRow; i < dataTableProcessor.RawRowCount; i++)
            {
                int index = i - startRow;

                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    stringBuilder.AppendLine().AppendLine();
                }

                stringBuilder
                    .AppendLine("        /// <summary>")
                    .AppendFormat("        /// {0}", dataTableProcessor.GetValue(i, 2)).AppendLine()
                    .AppendLine("        /// </summary>")
                    .AppendFormat("        {0} = {1},", dataTableProcessor.GetValue(i, 3), dataTableProcessor.GetValue(i, 1));
            }
            return stringBuilder.ToString();
        }
    }
}


