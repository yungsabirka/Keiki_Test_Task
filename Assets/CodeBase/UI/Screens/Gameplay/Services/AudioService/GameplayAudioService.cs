using System;
using System.Collections.Generic;
using CodeBase.Data.Levels;
using CodeBase.Services.AudioSystem.AudioSystem;
using CodeBase.Services.AudioSystem.Data;
using CodeBase.UI.Screens.Gameplay.Services.HintsTimerService;
using Cysharp.Threading.Tasks;
namespace CodeBase.UI.Screens.Gameplay.Services.AudioService
{
    public class GameplayAudioService : IGameplayAudioService
    {
        private readonly IAudioSystem _audioSystem;
        private readonly List<AudioType> _rewardAudioTypes;

        public GameplayAudioService(IAudioSystem audioSystem, IHintsTimerService hintsTimerService)
        {
            _audioSystem = audioSystem;
            _rewardAudioTypes = new List<AudioType>
            {
                AudioType.Awesome,
                AudioType.Excellent,
                AudioType.Good,
            };
        }

        public async UniTask PlayRandomRewardAudioAsync()
        {
            AudioType randomAudioType = _rewardAudioTypes[UnityEngine.Random.Range(0, _rewardAudioTypes.Count)];
            await _audioSystem.PlayOneShotAsync(randomAudioType);
        }

        public async UniTask PlayLevelStartAudioAsync(LevelType levelType)
        {
            switch (levelType)
            {
                case LevelType.Letter:
                    await _audioSystem.PlayOneShotAsync(AudioType.FollowLetter);
                    break;
                case LevelType.Number:
                case LevelType.Shape:
                    await _audioSystem.PlayOneShotAsync(AudioType.FollowNumber);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(levelType), levelType, null);
            }
        }
    }
}