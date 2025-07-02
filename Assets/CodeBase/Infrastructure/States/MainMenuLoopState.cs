using CodeBase.Infrastructure.Scenes;
using CodeBase.Infrastructure.States.Core;
using CodeBase.Infrastructure.UI;
using CodeBase.Services.AssetsSystem;
using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States
{
    public class MainMenuLoopState : IState
    {
        private readonly IUIRootFactory _uiRootFactory;
        private readonly IAssetsProvider _assetsProvider;
        
        public MainMenuLoopState(IUIRootFactory uiRootFactory, IAssetsProvider assetsProvider)
        {
            _uiRootFactory = uiRootFactory;
            _assetsProvider = assetsProvider;
        }

        public async UniTask Enter()
        {
            UIRoot uiRoot = await _uiRootFactory.GetUIRoot();
            uiRoot.HideLoadingScreen();
        }

        public async UniTask Exit()
        {
            UIRoot uiRoot = await _uiRootFactory.GetUIRoot();
            uiRoot.ShowLoadingScreen();
            uiRoot.ClearSceneUI();
            _assetsProvider.CleanUp();
        }
    }
}