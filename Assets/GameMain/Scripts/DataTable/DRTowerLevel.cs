//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-08-22 00:54:14.845
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
    /// 炮塔等级配置表。
    /// </summary>
    public class DRTowerLevel : DataRowBase
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
        /// 获取炮塔描述Id。
        /// </summary>
        public string DesId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取炮塔描述Id。
        /// </summary>
        public string UpgradeDesId
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
        /// 获取炮弹数据。
        /// </summary>
        public int ProjectileData
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取开火频率。
        /// </summary>
        public float FireRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取范围。
        /// </summary>
        public float Range
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取减速率。
        /// </summary>
        public float SpeedDownRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取生成能量。
        /// </summary>
        public float EnergyRaise
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取生成能量。
        /// </summary>
        public float EnergyRaiseRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取建造能量。
        /// </summary>
        public int BuildEnergy
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取出售能量。
        /// </summary>
        public int SellEnergy
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
            DesId = columnStrings[index++];
            UpgradeDesId = columnStrings[index++];
            EntityId = int.Parse(columnStrings[index++]);
            ProjectileData = int.Parse(columnStrings[index++]);
            FireRate = float.Parse(columnStrings[index++]);
            Range = float.Parse(columnStrings[index++]);
            SpeedDownRate = float.Parse(columnStrings[index++]);
            EnergyRaise = float.Parse(columnStrings[index++]);
            EnergyRaiseRate = float.Parse(columnStrings[index++]);
            BuildEnergy = int.Parse(columnStrings[index++]);
            SellEnergy = int.Parse(columnStrings[index++]);

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
                    DesId = binaryReader.ReadString();
                    UpgradeDesId = binaryReader.ReadString();
                    EntityId = binaryReader.Read7BitEncodedInt32();
                    ProjectileData = binaryReader.Read7BitEncodedInt32();
                    FireRate = binaryReader.ReadSingle();
                    Range = binaryReader.ReadSingle();
                    SpeedDownRate = binaryReader.ReadSingle();
                    EnergyRaise = binaryReader.ReadSingle();
                    EnergyRaiseRate = binaryReader.ReadSingle();
                    BuildEnergy = binaryReader.Read7BitEncodedInt32();
                    SellEnergy = binaryReader.Read7BitEncodedInt32();
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
