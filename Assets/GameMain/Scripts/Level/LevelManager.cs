using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    public class LevelManager : MonoBehaviour
    {
        [System.Serializable]
        public class LevelPathConfig
        {
            public LevelPath levelPath;
            public float weight;
        }

        [SerializeField]
        private LevelPathConfig[] levelPathConfigs;
        private float totalWeight;

        private void Awake()
        {
            for (int i = 0; i < levelPathConfigs.Length; i++)
            {
                totalWeight += levelPathConfigs[i].weight;
            }
        }

        public LevelPath GetLevelPath()
        {
            float random = Random.Range(0f, totalWeight);

            float sum = 0;

            for (int i = 0; i < levelPathConfigs.Length; i++)
            {
                sum += levelPathConfigs[i].weight;

                if (sum > random)
                    return levelPathConfigs[i].levelPath;
            }

            return null;
        }

        public Transform GetStartPathNode()
        {
            LevelPath levelPath = GetLevelPath();

            if (levelPath == null || levelPath.PathNodes.Length <= 0)
                return null;

            return levelPath.PathNodes[0];
        }

    }
}