//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-08-22 00:54:14.843
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
        /// 获取血量。
        /// </summary>
        public float MaxHP
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

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            NameId = columnStrings[index++];
            Icon = columnStrings[index++];
            PreviewEntityId = int.Parse(columnStrings[index++]);
            EntityId = int.Parse(columnStrings[index++]);
            ProjectileEntityId = int.Parse(columnStrings[index++]);
            ProjectileType = columnStrings[index++];
            IsMultiAttack = bool.Parse(columnStrings[index++]);
            MaxHP = float.Parse(columnStrings[index++]);
                Dimensions = DataTableExtension.ParseInt32Array(columnStrings[index++]);
            Type = columnStrings[index++];
                Levels = DataTableExtension.ParseInt32Array(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    NameId = binaryReader.ReadString();
                    Icon = binaryReader.ReadString();
                    PreviewEntityId = binaryReader.Read7BitEncodedInt32();
                    EntityId = binaryReader.Read7BitEncodedInt32();
                    ProjectileEntityId = binaryReader.Read7BitEncodedInt32();
                    ProjectileType = binaryReader.ReadString();
                    IsMultiAttack = binaryReader.ReadBoolean();
                    MaxHP = binaryReader.ReadSingle();
                        Dimensions = binaryReader.ReadInt32Array();
                    Type = binaryReader.ReadString();
                        Levels = binaryReader.ReadInt32Array();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
