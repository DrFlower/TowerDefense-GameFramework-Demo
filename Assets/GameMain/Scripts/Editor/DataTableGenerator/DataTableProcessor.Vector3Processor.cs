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
        private sealed class Vector3Processor : GenericDataProcessor<Vector3>
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
                    return "Vector3";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "vector3",
                    "unityengine.vector3"
                };
            }

            public override Vector3 Parse(string value)
            {
                string[] splitedValue = value.Split(',');
                return new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]));
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                Vector3 vector3 = Parse(value);
                binaryWriter.Write(vector3.x);
                binaryWriter.Write(vector3.y);
                binaryWriter.Write(vector3.z);
            }
        }
    }
}
