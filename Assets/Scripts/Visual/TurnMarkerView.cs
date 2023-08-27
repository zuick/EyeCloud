using UnityEngine;

namespace Game.Visual
{
    public class TurnMarkerView : MonoBehaviour
    {
        private Transform target;

        public void SetTarget(Transform target)
        {
            if (target != null)
            {
                transform.position = target.position;
            }

            this.target = target;
            gameObject.SetActive(target != null);
        }

        private void Update()
        {
            if (target == null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
