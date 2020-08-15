//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;
using UnityEngine;

namespace Flower.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class QuaternionProcessor : GenericDataProcessor<Quaternion>
        {
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "Quaternion";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "quaternion",
                    "unityengine.quaternion"
                };
            }

            public override Quaternion Parse(string value)
            {
                string[] splitedValue = value.Split(',');
                return new Quaternion(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                Quaternion quaternion = Parse(value);
                binaryWriter.Write(quaternion.x);
                binaryWriter.Write(quaternion.y);
                binaryWriter.Write(quaternion.z);
                binaryWriter.Write(quaternion.w);
            }
        }
    }
}
