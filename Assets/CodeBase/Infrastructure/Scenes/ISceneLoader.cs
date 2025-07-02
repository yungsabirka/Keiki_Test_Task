using System;
using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.Scenes
{
    public interface ISceneLoader
    {
        UniTask LoadAsync(string nextScene, Func<UniTask> onLoaded = null);
    }
}