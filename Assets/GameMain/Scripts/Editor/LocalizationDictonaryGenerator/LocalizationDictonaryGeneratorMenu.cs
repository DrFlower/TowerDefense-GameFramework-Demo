using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Flower
{
    public class LocalizationDictonaryGeneratorMenu
    {
        [MenuItem("Tools/Generate Localization Dictionary")]
        private static void GenerateLocalizationDictionary()
        {
            LocalizationDictonaryGenerator.GenerateEnglishLocalizationDictionary();
        }
    }
}


