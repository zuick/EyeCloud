using System;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    public class CoroutineProvider : MonoBehaviour
    {
        public static CoroutineProvider Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}
