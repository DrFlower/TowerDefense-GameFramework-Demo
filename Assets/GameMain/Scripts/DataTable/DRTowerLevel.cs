//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-07-06 01:33:59.544
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
        /// 获取伤害。
        /// </summary>
        public float Damage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取溅射伤害。
        /// </summary>
        public float SplashDamage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取溅射范围。
        /// </summary>
        public float SplashRange
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
                DesId = columnTexts[index++];
                UpgradeDesId = columnTexts[index++];
                EntityId = int.Parse(columnTexts[index++]);
                Damage = float.Parse(columnTexts[index++]);
                SplashDamage = float.Parse(columnTexts[index++]);
                SplashRange = float.Parse(columnTexts[index++]);
                FireRate = float.Parse(columnTexts[index++]);
                Range = float.Parse(columnTexts[index++]);
                SpeedDownRate = float.Parse(columnTexts[index++]);
                EnergyRaise = float.Parse(columnTexts[index++]);
                EnergyRaiseRate = float.Parse(columnTexts[index++]);
                BuildEnergy = int.Parse(columnTexts[index++]);
                SellEnergy = int.Parse(columnTexts[index++]);
            }
            else if (dataType == typeof(byte[]))
            {
                string[] strings = (string[])dataTableUserData;
                using (MemoryStream memoryStream = new MemoryStream((byte[])dataRowSegment.Data, dataRowSegment.Offset, dataRowSegment.Length, false))
                {
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        m_Id = binaryReader.Read7BitEncodedInt32();
                        DesId = strings[binaryReader.Read7BitEncodedInt32()];
                        UpgradeDesId = strings[binaryReader.Read7BitEncodedInt32()];
                        EntityId = binaryReader.Read7BitEncodedInt32();
                        Damage = binaryReader.ReadSingle();
                        SplashDamage = binaryReader.ReadSingle();
                        SplashRange = binaryReader.ReadSingle();
                        FireRate = binaryReader.ReadSingle();
                        Range = binaryReader.ReadSingle();
                        SpeedDownRate = binaryReader.ReadSingle();
                        EnergyRaise = binaryReader.ReadSingle();
                        EnergyRaiseRate = binaryReader.ReadSingle();
                        BuildEnergy = binaryReader.Read7BitEncodedInt32();
                        SellEnergy = binaryReader.Read7BitEncodedInt32();
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
