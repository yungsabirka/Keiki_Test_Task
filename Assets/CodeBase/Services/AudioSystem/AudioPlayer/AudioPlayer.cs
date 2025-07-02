using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
namespace CodeBase.Services.AudioSystem.AudioPlayer
{
    public class AudioPlayer : IAudioPlayer
    {
        private GameObject _audioRoot;
        private AudioSource _audioSource;

        public void Initialize()
        {
            _audioRoot = new GameObject("AudioSystem_Runtime");
            Object.DontDestroyOnLoad(_audioRoot);

            _audioSource = _audioRoot.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        public async UniTask PlayOneShotAsync(AudioClip clip, float volume = 0.2f)
        {
            if (clip == null)
                throw new ArgumentNullException(nameof(clip));

            _audioSource.PlayOneShot(clip, volume);
            await UniTask.Delay(TimeSpan.FromSeconds(clip.length));
        }

        public void Stop() =>
            _audioSource.Stop();
    }
}