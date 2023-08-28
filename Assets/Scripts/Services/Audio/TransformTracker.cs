using UnityEngine;
using UnityEditor;

namespace Game.Audio
{
    public class TransformTracker : MonoBehaviour
    {
        public Transform Target;

        private void LateUpdate()
        {
            if (Target != null)
            {
                transform.position = Target.position;
            }
        }
    }
}