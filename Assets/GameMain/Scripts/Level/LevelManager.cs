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

        [SerializeField]
        private Collider[] environmentColliders;

        public Collider[] EnvironmentColliders
        {
            get
            {
                return environmentColliders;
            }
        }

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

            for (int i = 0; i < levelPathConfigs.Length; i++)
            {
                if (levelPathConfigs[i].weight > random)
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