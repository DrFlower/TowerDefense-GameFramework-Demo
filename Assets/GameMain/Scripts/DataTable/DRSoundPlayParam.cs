//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-05-04 21:10:59.767
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
    /// 声音播放配置表。
    /// </summary>
    public class DRSoundPlayParam : DataRowBase
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
        /// 获取播放开始时间。
        /// </summary>
        public float Time
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取静音。
        /// </summary>
        public bool Mute
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否循环。
        /// </summary>
        public bool Loop
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取优先级（默认0，128最高，-128最低）。
        /// </summary>
        public int Priority
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取音量（0~1）。
        /// </summary>
        public float Volume
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取淡入时间。
        /// </summary>
        public float FadeInSeconds
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取Pitch。
        /// </summary>
        public float Pitch
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取PanStereo。
        /// </summary>
        public float PanStereo
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音空间混合量（0为2D，1为3D，中间值混合效果）。
        /// </summary>
        public float SpatialBlend
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取声音最大距离。
        /// </summary>
        public float MaxDistance
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取DopplerLevel。
        /// </summary>
        public float DopplerLevel
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
                Time = float.Parse(columnTexts[index++]);
                Mute = bool.Parse(columnTexts[index++]);
                Loop = bool.Parse(columnTexts[index++]);
                Priority = int.Parse(columnTexts[index++]);
                Volume = float.Parse(columnTexts[index++]);
                FadeInSeconds = float.Parse(columnTexts[index++]);
                Pitch = float.Parse(columnTexts[index++]);
                PanStereo = float.Parse(columnTexts[index++]);
                SpatialBlend = float.Parse(columnTexts[index++]);
                MaxDistance = float.Parse(columnTexts[index++]);
                DopplerLevel = float.Parse(columnTexts[index++]);
            }
            else if (dataType == typeof(byte[]))
            {
                string[] strings = (string[])dataTableUserData;
                using (MemoryStream memoryStream = new MemoryStream((byte[])dataRowSegment.Data, dataRowSegment.Offset, dataRowSegment.Length, false))
                {
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        m_Id = binaryReader.Read7BitEncodedInt32();
                        Time = binaryReader.ReadSingle();
                        Mute = binaryReader.ReadBoolean();
                        Loop = binaryReader.ReadBoolean();
                        Priority = binaryReader.Read7BitEncodedInt32();
                        Volume = binaryReader.ReadSingle();
                        FadeInSeconds = binaryReader.ReadSingle();
                        Pitch = binaryReader.ReadSingle();
                        PanStereo = binaryReader.ReadSingle();
                        SpatialBlend = binaryReader.ReadSingle();
                        MaxDistance = binaryReader.ReadSingle();
                        DopplerLevel = binaryReader.ReadSingle();
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
