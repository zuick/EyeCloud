using System;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    public class VisualConfig : MonoBehaviour
    {
        public float GridScale = 10f;

        public static VisualConfig Instance;

        private void Awake()
        {
            Instance = this;
        }

        public static Vector3 ToLevelLocal(IntPoint gamePosition)
        {
            return new Vector3(gamePosition.X * Instance.GridScale, 0f, gamePosition.Y * Instance.GridScale);
        }
    }
}
