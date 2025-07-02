using System;
using CodeBase.Services.AssetsSystem;
using CodeBase.Services.AudioSystem.AudioPlayer;
using Cysharp.Threading.Tasks;
using UnityEngine;
using AudioConfiguration = CodeBase.Services.AudioSystem.Data.AudioConfiguration;
using AudioType = CodeBase.Services.AudioSystem.Data.AudioType;
using static CodeBase.Services.AssetsSystem.Data.AssetsSystemConstants;
namespace CodeBase.Services.AudioSystem.AudioSystem
{
    public class AudioSystem : IAudioSystem
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IAudioPlayer _audioPlayer;
        private AudioConfiguration _audioConfiguration;
        private AudioListener _audioListener;

        public AudioSystem(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
            _audioPlayer = new AudioPlayer.AudioPlayer();
        }

        public async UniTask Initialize()
        {
            _audioConfiguration = await _assetsProvider.LoadAsync<AudioConfiguration>(AudioConfigurationKey);
            _audioPlayer.Initialize();
        }

        public async UniTask PlayOneShotAsync(AudioType audioType)
        {
            if(audioType == AudioType.None)
                throw new ArgumentException("Can not play a non-audio type");

            Stop();
            await _audioPlayer.PlayOneShotAsync(GetAudioClip(audioType));
        }
        
        public void Stop() =>
            _audioPlayer.Stop();

        private AudioClip GetAudioClip(AudioType audioType) => 
            _audioConfiguration.GetAudioClip(audioType);
    }

}