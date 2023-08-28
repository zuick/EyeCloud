using UnityEngine;
using System.Linq;
using Game.Services;

namespace Game.Audio
{
    public class SoundHandler : MonoBehaviour
    {
        public SoundId sound;
        public SoundId[] variants;

        public bool playOnEnable;
        public bool playOnStart;
        public bool noSpatialBlend;
        public bool trackTransform;

        private System.Guid id;
        private SoundManager soundManager;
        private SoundId[] allSounds;
        private SoundId lastPlayerSoundId = SoundId.None;

        public void Awake()
        {
            soundManager = DiContainerService.Instance.Resolve<SoundManager>();
        }

        private void ValidateSounds()
        {
            if (allSounds != null && allSounds.Length != 0)
            {
                return;
            }

            if (variants.Length > 0)
            {
                var allSoundsList = variants.ToList();
                allSoundsList.Add(sound);
                allSounds = allSoundsList.ToArray();
            }
            else
            {
                allSounds = new SoundId[] { sound };
            }
        }

        private void OnEnable()
        {
            if (playOnEnable)
                Play();
        }

        private void Start()
        {
            if (playOnStart)
                Play();
        }

        private void OnDisable()
        {
            if (lastPlayerSoundId != SoundId.None)
            {
                var setting = soundManager.GetAudioSetting(lastPlayerSoundId);

                if (setting.loop && (playOnEnable || playOnStart))
                    Stop();
            }
        }

        public void Play()
        {
            ValidateSounds();
            if (soundManager != null)
            {
                lastPlayerSoundId = allSounds[Random.Range(0, allSounds.Length)];
                if (noSpatialBlend)
                    id = soundManager.Play(lastPlayerSoundId);
                else if (trackTransform)
                    id = soundManager.Play(lastPlayerSoundId, transform);
                else
                    id = soundManager.Play(lastPlayerSoundId, transform.position);
            }
        }

        public void Stop()
        {
            soundManager.Stop(id);
        }
    }
}