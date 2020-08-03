using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Flower
{

    [Serializable]
    public class WeightedSound
    {
        public EnumSound sound;
        public int weight = 1;
    }

    public class RandomSound : MonoBehaviour
    {
        public WeightedSound[] weightedItems;

        private int TotalWeight
        {
            get
            {
                int result = 0;
                for (int i = 0; i < weightedItems.Length; i++)
                {
                    result += weightedItems[i].weight;
                }
                return result;
            }
        }

        public EnumSound GetRandomSound()
        {
            float random = UnityEngine.Random.Range(0f, TotalWeight);

            int sum = 0;

            for (int i = 0; i < weightedItems.Length; i++)
            {
                sum += weightedItems[i].weight;
                if (sum > random)
                    return weightedItems[i].sound;
            }

            return weightedItems[0].sound;
        }

    }
}

