using CodeBase.Data.Levels;
using Cysharp.Threading.Tasks;
namespace CodeBase.UI.Screens.Gameplay.Services.AudioService
{
    public interface IGameplayAudioService
    {
        UniTask PlayRandomRewardAudioAsync();

        UniTask PlayLevelStartAudioAsync(LevelType levelType);
    }
}