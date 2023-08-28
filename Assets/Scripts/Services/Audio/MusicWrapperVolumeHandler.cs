
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    public class MusicWrapperVolumeHandler : MonoBehaviour
    {
        public AudioMixerGroup output;
        public AudioMixerSettings audioMixerSettings;
        public float duration = 1f;
        public bool muteOnStart;

        private void Start()
        {
            if (muteOnStart)
            {
                MuteImmediately();
            }
        }

        public void Mute()
        {
            if (GetCurrentVolume() != audioMixerSettings.minVolume)
            {
                StopAllCoroutines();
                StartCoroutine(FadeCoroutine(audioMixerSettings.minVolume));
            }
        }

        public void MuteImmediately()
        {
            StopAllCoroutines();
            SetVolume(audioMixerSettings.minVolume);
        }

        public void Unmute()
        {
            StopAllCoroutines();
            StartCoroutine(FadeCoroutine(0f));
        }

        private float GetCurrentVolume()
        {
            output.audioMixer.GetFloat(audioMixerSettings.musicWrapperVolumeParamName, out var volume);
            return volume;
        }

        private void SetVolume(float v)
        {
            output.audioMixer.SetFloat(audioMixerSettings.musicWrapperVolumeParamName, v);
        }

        IEnumerator FadeCoroutine(float targetVolume)
        {
            var volume = GetCurrentVolume();
            var delta = targetVolume - volume;
            while ((delta < 0f && volume > targetVolume) || (delta > 0f && volume < targetVolume))
            {
                volume += delta * Time.deltaTime / duration;
                SetVolume(volume);
                yield return new WaitForEndOfFrame();
            }

            SetVolume(targetVolume);
        }

        private void OnDestroy()
        {
            SetVolume(0f);
        }
    }
}