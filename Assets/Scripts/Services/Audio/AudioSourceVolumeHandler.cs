using UnityEngine;
using System.Collections;

namespace Game.Audio
{
	public class AudioSourceVolumeHandler : MonoBehaviour
	{
		public AudioSource audioSource;
		public float duration = 1f;

        public void Mute()
        {
			StopAllCoroutines();
			StartCoroutine(FadeCoroutine(0f));
        }

        public void Unmute()
        {
			StopAllCoroutines();
			StartCoroutine(FadeCoroutine(1f));
		}

		IEnumerator FadeCoroutine(float targetVolume)
		{
			var volume = audioSource.volume;
			var delta = targetVolume - volume;
			while ((delta < 0f && volume > targetVolume) || (delta > 0f && volume < targetVolume))
			{
				volume += delta * Time.deltaTime / duration;
				audioSource.volume = volume;
				yield return new WaitForEndOfFrame();
			}

			audioSource.volume = targetVolume;
		}
	}
}