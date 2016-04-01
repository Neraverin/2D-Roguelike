using UnityEngine;

namespace Assets.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource EfxSource;
        public AudioSource MusicSource;

        public static SoundManager Instance;

        public float LowPitchRange = 0.95f;
        public float HighPitchRange = 1.05f;

        void Awake ()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public void PlaySingle(AudioClip clip)
        {
            EfxSource.clip = clip;
            EfxSource.Play();
        }

        public void RandomizeSfx(params AudioClip[] clips)
        {
            var randomIndex = Random.Range(0, clips.Length);
            var randomPitch = Random.Range(LowPitchRange, HighPitchRange);

            EfxSource.pitch = randomPitch;
            PlaySingle(clips[randomIndex]);
        }

    }
} // namespace
