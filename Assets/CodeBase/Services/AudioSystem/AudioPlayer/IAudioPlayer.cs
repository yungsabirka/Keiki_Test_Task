using Cysharp.Threading.Tasks;
using UnityEngine;
namespace CodeBase.Services.AudioSystem.AudioPlayer
{
    public interface IAudioPlayer
    {
        void Initialize();

        UniTask PlayOneShotAsync(AudioClip clip, float volume = 0.2f);

        void Stop();
    }
}