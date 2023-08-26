using UnityEngine;

namespace Game.Common
{
    public class AutoRotateInRange : MonoBehaviour
    {
        public enum RotationAxis { x, y, z };
        public Transform target;
        public RotationAxis axis;
        public float amplitude = 90f;
        public float period = 1f;
        public bool reverse;

        private Vector3 initialRotation;
        private float counter = 0f;

        private void Awake()
        {
            if (target == null)
                target = transform;

            initialRotation = target.localEulerAngles;
        }

        private void Update()
        {
            var delta = amplitude * Mathf.Sin(counter) * (reverse ? -1f : 1f);

            var newRotation = initialRotation;
            if (axis == RotationAxis.x)
                newRotation.x += delta;
            if (axis == RotationAxis.y)
                newRotation.y += delta;
            if (axis == RotationAxis.z)
                newRotation.z += delta;

            target.localEulerAngles = newRotation;
            counter += Time.deltaTime / period;
        }
    }
}