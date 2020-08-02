//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-08-02 12:35:34.243
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Flower
{
    /// <summary>
    /// 炮塔配置表。
    /// </summary>
    public class DRTower : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取配置编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取炮塔名字Id。
        /// </summary>
        public string NameId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取图标名称。
        /// </summary>
        public string Icon
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取预览实体编号。
        /// </summary>
        public int PreviewEntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取炮弹实体编号。
        /// </summary>
        public int ProjectileEntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取炮弹类型。
        /// </summary>
        public string ProjectileType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否同时攻击多个敌人。
        /// </summary>
        public bool IsMultiAttack
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取占用面积。
        /// </summary>
        public int[] Dimensions
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取脚本类型。
        /// </summary>
        public string Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取等级列表。
        /// </summary>
        public int[] Levels
        {
            get;
            private set;
        }

        public override bool ParseDataRow(GameFrameworkDataSegment dataRowSegment, object dataTableUserData)
        {
            Type dataType = dataRowSegment.DataType;
            if (dataType == typeof(string))
            {
                string[] columnTexts = ((string)dataRowSegment.Data).Substring(dataRowSegment.Offset, dataRowSegment.Length).Split(DataTableExtension.DataSplitSeparators);
                for (int i = 0; i < columnTexts.Length; i++)
                {
                    columnTexts[i] = columnTexts[i].Trim(DataTableExtension.DataTrimSeparators);
                }

                int index = 0;
                index++;
                m_Id = int.Parse(columnTexts[index++]);
                index++;
                NameId = columnTexts[index++];
                Icon = columnTexts[index++];
                PreviewEntityId = int.Parse(columnTexts[index++]);
                EntityId = int.Parse(columnTexts[index++]);
                ProjectileEntityId = int.Parse(columnTexts[index++]);
                ProjectileType = columnTexts[index++];
                IsMultiAttack = bool.Parse(columnTexts[index++]);
                Dimensions = DataTableExtension.ParseInt32Array(columnTexts[index++]);
                Type = columnTexts[index++];
                Levels = DataTableExtension.ParseInt32Array(columnTexts[index++]);
            }
            else if (dataType == typeof(byte[]))
            {
                string[] strings = (string[])dataTableUserData;
                using (MemoryStream memoryStream = new MemoryStream((byte[])dataRowSegment.Data, dataRowSegment.Offset, dataRowSegment.Length, false))
                {
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        m_Id = binaryReader.Read7BitEncodedInt32();
                        NameId = strings[binaryReader.Read7BitEncodedInt32()];
                        Icon = strings[binaryReader.Read7BitEncodedInt32()];
                        PreviewEntityId = binaryReader.Read7BitEncodedInt32();
                        EntityId = binaryReader.Read7BitEncodedInt32();
                        ProjectileEntityId = binaryReader.Read7BitEncodedInt32();
                        ProjectileType = strings[binaryReader.Read7BitEncodedInt32()];
                        IsMultiAttack = binaryReader.ReadBoolean();
                        Dimensions = binaryReader.ReadInt32Array();
                        Type = strings[binaryReader.Read7BitEncodedInt32()];
                        Levels = binaryReader.ReadInt32Array();
                    }
                }
            }
            else
            {
                Log.Warning("Can not parse data row which type '{0}' is invalid.", dataType.FullName);
                return false;
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
