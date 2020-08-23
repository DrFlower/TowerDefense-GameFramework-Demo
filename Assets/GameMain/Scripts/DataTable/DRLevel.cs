//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-08-22 00:54:14.824
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
    /// 关卡配置。
    /// </summary>
    public class DRLevel : DataRowBase
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
        /// 获取关卡名字Id。
        /// </summary>
        public string NameId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取关卡描述Id。
        /// </summary>
        public string DescriptionId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取场景Id。
        /// </summary>
        public int SceneId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源组名字。
        /// </summary>
        public string ResourceGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取初始能量。
        /// </summary>
        public int InitEnergy
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取玩家位置。
        /// </summary>
        public Vector3 PlayerPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取玩家旋转。
        /// </summary>
        public Vector3 PlayerQuaternion
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取波次ID。
        /// </summary>
        public int[] WaveIds
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取关卡允许使用的炮塔。
        /// </summary>
        public int[] AllowTowers
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
            DescriptionId = columnStrings[index++];
            SceneId = int.Parse(columnStrings[index++]);
            ResourceGroupName = columnStrings[index++];
            InitEnergy = int.Parse(columnStrings[index++]);
            PlayerPosition = DataTableExtension.ParseVector3(columnStrings[index++]);
            PlayerQuaternion = DataTableExtension.ParseVector3(columnStrings[index++]);
                WaveIds = DataTableExtension.ParseInt32Array(columnStrings[index++]);
                AllowTowers = DataTableExtension.ParseInt32Array(columnStrings[index++]);

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
                    DescriptionId = binaryReader.ReadString();
                    SceneId = binaryReader.Read7BitEncodedInt32();
                    ResourceGroupName = binaryReader.ReadString();
                    InitEnergy = binaryReader.Read7BitEncodedInt32();
                    PlayerPosition = binaryReader.ReadVector3();
                    PlayerQuaternion = binaryReader.ReadVector3();
                        WaveIds = binaryReader.ReadInt32Array();
                        AllowTowers = binaryReader.ReadInt32Array();
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
