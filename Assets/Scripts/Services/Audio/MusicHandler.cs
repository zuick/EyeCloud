using UnityEngine;
using Zenject;

namespace Game.Audio
{
    public class MusicHandler : MonoBehaviour
    {
        public MusicId[] music;
        public bool stopPrevious = true;
        public bool stopOnDisable = true;
        private MusicId current;
        private MusicManager musicManager;

        [Inject]
        public void Constract(MusicManager musicManager)
        {
            this.musicManager = musicManager;
        }

        private void OnEnable()
        {
            if (stopPrevious)
                musicManager.Stop();

            if (music.Length == 0)
                return;

            current = music[UnityEngine.Random.Range(0, music.Length)];
            if (music.Length > 0 && musicManager)
                musicManager.Play(current);
        }

        private void OnDisable()
        {
            if (music.Length > 0 && musicManager != null && stopOnDisable)
                musicManager.Stop();
        }
    }
}