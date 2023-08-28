using UnityEngine.UI;
using UnityEngine;
namespace Game.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image progressImage;

        public void Refresh(float current, float max)
        {
            progressImage.fillAmount = current / max;
        }
    }
}