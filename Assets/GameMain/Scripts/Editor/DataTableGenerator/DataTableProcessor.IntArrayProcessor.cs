using System.IO;
using UnityEngine;

namespace Flower.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class IntArrayrocessor : GenericDataProcessor<int[]>
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
                    return "int[]";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "int[]",
                    "System.Int32[]"
                };
            }

            public override int[] Parse(string value)
            {
                string[] splitValue = value.Split(',');
                int[] result = new int[splitValue.Length];
                for (int i = 0; i < splitValue.Length; i++)
                {
                    result[i] = int.Parse(splitValue[i]);
                }

                return result;
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                int[] intArray = Parse(value);
                binaryWriter.Write(intArray.Length);
                foreach (var elementValue in intArray)
                {
                    binaryWriter.Write(elementValue);
                }
            }
        }
    }
}
