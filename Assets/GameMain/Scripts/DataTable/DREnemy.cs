//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-07-26 20:55:15.054
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
    /// 敌人配置表。
    /// </summary>
    public class DREnemy : DataRowBase
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
        /// 获取敌人名字Id。
        /// </summary>
        public string NameId
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
        /// 获取血量。
        /// </summary>
        public float MaxHP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取移动速度。
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡特效实体Id。
        /// </summary>
        public int DeadEffcetEntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡特效偏移。
        /// </summary>
        public Vector3 DeadEffectOffset
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
                EntityId = int.Parse(columnTexts[index++]);
                MaxHP = float.Parse(columnTexts[index++]);
                Speed = float.Parse(columnTexts[index++]);
                DeadEffcetEntityId = int.Parse(columnTexts[index++]);
                DeadEffectOffset = DataTableExtension.ParseVector3(columnTexts[index++]);
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
                        EntityId = binaryReader.Read7BitEncodedInt32();
                        MaxHP = binaryReader.ReadSingle();
                        Speed = binaryReader.ReadSingle();
                        DeadEffcetEntityId = binaryReader.Read7BitEncodedInt32();
                        DeadEffectOffset = binaryReader.ReadVector3();
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
