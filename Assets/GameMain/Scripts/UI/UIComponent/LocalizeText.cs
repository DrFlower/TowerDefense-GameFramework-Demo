using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Flower
{
    [RequireComponent(typeof(Text))]
    [DisallowMultipleComponent]
    public class LocalizeText : MonoBehaviour
    {
        private Text text;

        public string dictionaryKey;

        private void Awake()
        {
            text = gameObject.GetComponent<Text>();
        }

        private void OnEnable()
        {
            text.text = GameEntry.Localization.GetString(dictionaryKey);
        }
    }

}