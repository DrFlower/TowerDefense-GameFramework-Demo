//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-08-14 01:28:19.700
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
        /// 获取脚本类型。
        /// </summary>
        public string Type
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
        /// 获取对基地伤害。
        /// </summary>
        public int Damage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对炮塔伤害。
        /// </summary>
        public float TowerDamage
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
        /// 获取是否同时攻击多个敌人。
        /// </summary>
        public bool IsMultiAttack
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
        /// 获取死亡时玩家获得能量。
        /// </summary>
        public float AddEnergy
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
                Type = columnTexts[index++];
                MaxHP = float.Parse(columnTexts[index++]);
                Damage = int.Parse(columnTexts[index++]);
                TowerDamage = float.Parse(columnTexts[index++]);
                ProjectileEntityId = int.Parse(columnTexts[index++]);
                ProjectileType = columnTexts[index++];
                ProjectileData = int.Parse(columnTexts[index++]);
                FireRate = float.Parse(columnTexts[index++]);
                Range = float.Parse(columnTexts[index++]);
                IsMultiAttack = bool.Parse(columnTexts[index++]);
                Speed = float.Parse(columnTexts[index++]);
                AddEnergy = float.Parse(columnTexts[index++]);
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
                        Type = strings[binaryReader.Read7BitEncodedInt32()];
                        MaxHP = binaryReader.ReadSingle();
                        Damage = binaryReader.Read7BitEncodedInt32();
                        TowerDamage = binaryReader.ReadSingle();
                        ProjectileEntityId = binaryReader.Read7BitEncodedInt32();
                        ProjectileType = strings[binaryReader.Read7BitEncodedInt32()];
                        ProjectileData = binaryReader.Read7BitEncodedInt32();
                        FireRate = binaryReader.ReadSingle();
                        Range = binaryReader.ReadSingle();
                        IsMultiAttack = binaryReader.ReadBoolean();
                        Speed = binaryReader.ReadSingle();
                        AddEnergy = binaryReader.ReadSingle();
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
