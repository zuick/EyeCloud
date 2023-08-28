using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Audio
{
    [Serializable]
    public class MusicSetting : AudioSetting
    {
        public MusicId id;
        public override string Name => id.ToString();
        public override int IntId => (int)id;
    }

    public class MusicManager : AbstractAudioManager<MusicSetting, MusicId>
    {
        public float fadeOutDuration = 1f;
        public float fadeInDuration = 0.5f;

        public bool fadeIn = false;
        public bool fadeOut = true;

        public override string ParamName => audioMixerSettings.musicVolumeParamName;
        public override string VolumePlayerPref => SystemPlayerPrefsKeys.MusicVolume.ToString();

        private bool fadingIn;
        private bool fadingOut;

        private MusicId currentMusicId;

        public override void SetVolume(float v)
        {
            StopAllCoroutines();
            fadingIn = false;
            fadingOut = false;
            base.SetVolume(v);
        }

        protected override MusicSetting GetAudioSetting(int id)
        {
            return new MusicSetting { id = (MusicId)id };
        }

        protected override void SetAudioSourceSettings(AudioSource audioSource, MusicSetting setting, Vector3 position, bool noSpatialBlend = false)
        {
            SetAudioSourceSettings(audioSource, setting.clip, position, true, false, true);
        }

        public override Guid Play(MusicId id)
        {
            if (id != currentMusicId)
            {
                StartCoroutine(PlayWithFadeCoroutine(id));
            }
            return Guid.Empty;
        }

        public Guid Play(MusicId id, bool playMuted)
        {
            if (id != currentMusicId)
            {
                StartCoroutine(PlayWithFadeCoroutine(id, playMuted));
            }
            return Guid.Empty;
        }

        public void Stop()
        {
            if (!fadingOut)
                StartCoroutine(StopWithFadeCoroutine());
        }

        public void Mute()
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutCoroutine());
        }

        public void Unmute()
        {
            StopAllCoroutines();
            StartCoroutine(FadeInCoroutine());
        }

        IEnumerator FadeInCoroutine()
        {
            fadingIn = true;
            var volume = audioMixerSettings.minVolume;
            var targetVolume = GetSavedVolume();
            var delta = targetVolume - volume;
            while (volume < targetVolume)
            {
                volume += delta * Time.deltaTime / fadeInDuration;
                output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, volume);
                yield return new WaitForEndOfFrame();
            }

            output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, targetVolume);
            fadingIn = false;
        }

        IEnumerator FadeOutCoroutine()
        {
            fadingOut = true;
            output.audioMixer.GetFloat(audioMixerSettings.musicVolumeParamName, out var volume);
            var targetVolume = audioMixerSettings.minVolume;
            var delta = targetVolume - volume;
            while (volume > targetVolume)
            {
                volume += delta * Time.deltaTime / fadeOutDuration;
                output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, volume);
                yield return new WaitForEndOfFrame();
            }

            output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, targetVolume);
            fadingOut = false;
        }

        IEnumerator StopWithFadeCoroutine()
        {
            if (!fadeOut)
            {
                StopAll();
                yield break;
            }

            fadingOut = true;
            output.audioMixer.GetFloat(audioMixerSettings.musicVolumeParamName, out var volume);
            var originalVolume = volume;
            var delta = originalVolume - audioMixerSettings.minVolume;

            while (volume > audioMixerSettings.minVolume)
            {
                volume -= delta * Time.deltaTime / fadeOutDuration;
                output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, volume);
                yield return new WaitForEndOfFrame();
            }

            StopAll();

            output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, originalVolume);

            fadingOut = false;
        }

        IEnumerator PlayWithFadeCoroutine(MusicId id, bool playMuted = false)
        {
            yield return new WaitWhile(() => fadingOut);

            if (!fadeIn)
            {
                if (playMuted)
                {
                    output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, audioMixerSettings.minVolume);
                }
                yield return new WaitForEndOfFrame();
                currentMusicId = id;
                base.Play(id);
                yield break;
            }

            fadingIn = true;

            var originalVolume = GetSavedVolume();
            var delta = originalVolume - audioMixerSettings.minVolume;

            var volume = audioMixerSettings.minVolume;
            output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, audioMixerSettings.minVolume);
            yield return new WaitForEndOfFrame();
            currentMusicId = id;
            base.Play(id);

            if (!playMuted)
            {
                while (volume < originalVolume)
                {
                    volume += delta * Time.deltaTime / fadeInDuration;
                    output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, volume);
                    yield return new WaitForEndOfFrame();
                }

                output.audioMixer.SetFloat(audioMixerSettings.musicVolumeParamName, originalVolume);
            }

            fadingIn = false;
        }
    }
}