using CodeBase.Services.AudioSystem.Data;
using Cysharp.Threading.Tasks;
namespace CodeBase.Services.AudioSystem.AudioSystem
{
    public interface IAudioSystem
    {
        UniTask Initialize();

        UniTask PlayOneShotAsync(AudioType audioType);
    }
}