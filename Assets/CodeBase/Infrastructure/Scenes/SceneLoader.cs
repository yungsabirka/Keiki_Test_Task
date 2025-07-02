using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
namespace CodeBase.Infrastructure.Scenes
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadAsync(string nextScene, Func<UniTask> onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                return;
            }

            await SceneManager
                .LoadSceneAsync(nextScene)
                .ToUniTask();

            onLoaded?.Invoke();
        }
    }
}