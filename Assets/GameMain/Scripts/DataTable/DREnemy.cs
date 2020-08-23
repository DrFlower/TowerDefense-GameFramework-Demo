//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-08-22 00:54:14.810
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
            EntityId = int.Parse(columnStrings[index++]);
            Type = columnStrings[index++];
            MaxHP = float.Parse(columnStrings[index++]);
            Damage = int.Parse(columnStrings[index++]);
            TowerDamage = float.Parse(columnStrings[index++]);
            ProjectileEntityId = int.Parse(columnStrings[index++]);
            ProjectileType = columnStrings[index++];
            ProjectileData = int.Parse(columnStrings[index++]);
            FireRate = float.Parse(columnStrings[index++]);
            Range = float.Parse(columnStrings[index++]);
            IsMultiAttack = bool.Parse(columnStrings[index++]);
            Speed = float.Parse(columnStrings[index++]);
            AddEnergy = float.Parse(columnStrings[index++]);

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
                    EntityId = binaryReader.Read7BitEncodedInt32();
                    Type = binaryReader.ReadString();
                    MaxHP = binaryReader.ReadSingle();
                    Damage = binaryReader.Read7BitEncodedInt32();
                    TowerDamage = binaryReader.ReadSingle();
                    ProjectileEntityId = binaryReader.Read7BitEncodedInt32();
                    ProjectileType = binaryReader.ReadString();
                    ProjectileData = binaryReader.Read7BitEncodedInt32();
                    FireRate = binaryReader.ReadSingle();
                    Range = binaryReader.ReadSingle();
                    IsMultiAttack = binaryReader.ReadBoolean();
                    Speed = binaryReader.ReadSingle();
                    AddEnergy = binaryReader.ReadSingle();
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
