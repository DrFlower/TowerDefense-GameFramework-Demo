using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flower
{
    [SerializeField]
    public class LevelPath : MonoBehaviour
    {
        [SerializeField]
        private Transform[] pathNodes;

        public Transform[] PathNodes
        {
            get
            {
                return pathNodes;
            }
        }
    }

}