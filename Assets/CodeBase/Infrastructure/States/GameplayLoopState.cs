using System;
using CodeBase.Infrastructure.States.Core;
using CodeBase.Infrastructure.UI;
using CodeBase.Services.AssetsSystem;
using CodeBase.Services.AudioSystem.AudioSystem;
using CodeBase.Services.ObjectsPool;
using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States
{
    public class GameplayLoopState : IPayloadedState<Action>
    {
        private readonly IUIRootFactory _uiRootFactory;
        private readonly IObjectsPool _objectsPool;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IAudioSystem _audioSystem;

        public GameplayLoopState(IUIRootFactory uiRootFactory, IObjectsPool objectsPool, IAssetsProvider assetsProvider,
            IAudioSystem audioSystem)
        {
            _uiRootFactory = uiRootFactory;
            _objectsPool = objectsPool;
            _assetsProvider = assetsProvider;
            _audioSystem = audioSystem;
        }

        public async UniTask Enter(Action onStartedLoop)
        {
            UIRoot uiRoot = await _uiRootFactory.GetUIRoot();
            uiRoot.HideLoadingScreen();
            onStartedLoop?.Invoke();
        }

        public async UniTask Exit()
        {
            _audioSystem.Stop();
            UIRoot uiRoot = await _uiRootFactory.GetUIRoot();
            uiRoot.ShowLoadingScreen();
            uiRoot.ClearSceneUI();
            _assetsProvider.CleanUp();
            _objectsPool.ClearPool();
        }
    }
}