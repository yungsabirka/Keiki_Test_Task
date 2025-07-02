using System;
using CodeBase.Infrastructure.States.Core;
using CodeBase.Infrastructure.UI;
using CodeBase.Services.AssetsSystem;
using CodeBase.Services.ObjectsPool;
using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States
{
    public class GameplayLoopState : IPayloadedState<Action>
    {
        private readonly IUIRootFactory _uiRootFactory;
        private readonly IObjectsPool _objectsPool;
        private readonly IAssetsProvider _assetsProvider;

        public GameplayLoopState(IUIRootFactory uiRootFactory, IObjectsPool objectsPool, IAssetsProvider assetsProvider)
        {
            _uiRootFactory = uiRootFactory;
            _objectsPool = objectsPool;
            _assetsProvider = assetsProvider;
        }

        public async UniTask Enter(Action onStartedLoop)
        {
            UIRoot uiRoot = await _uiRootFactory.GetUIRoot();
            uiRoot.HideLoadingScreen();
            onStartedLoop?.Invoke();
        }

        public async UniTask Exit()
        {
            UIRoot uiRoot = await _uiRootFactory.GetUIRoot();
            uiRoot.ShowLoadingScreen();
            uiRoot.ClearSceneUI();
            _assetsProvider.CleanUp();
            _objectsPool.ClearPool();
        }
    }
}