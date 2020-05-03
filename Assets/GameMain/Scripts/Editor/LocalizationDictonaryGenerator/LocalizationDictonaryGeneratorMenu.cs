using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Flower
{
    public class LocalizationDictonaryGeneratorMenu
    {
        [MenuItem("Tools/Generate Localizatio Dictionary")]
        private static void GenerateLocalizatioDictionary()
        {
            LocalizationDictonaryGenerator.GenerateEnglishLocalizationDictionary();
        }
    }
}


