using CodeBase.Infrastructure.Scenes;
using CodeBase.Infrastructure.States.Core;
using CodeBase.Infrastructure.UI;
using CodeBase.Services.AssetsSystem;
using CodeBase.Services.AudioSystem.AudioSystem;
using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States
{
    public class MainMenuLoopState : IState
    {
        private readonly IUIRootFactory _uiRootFactory;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IAudioSystem _audioSystem;

        public MainMenuLoopState(IUIRootFactory uiRootFactory, IAssetsProvider assetsProvider, IAudioSystem audioSystem)
        {
            _uiRootFactory = uiRootFactory;
            _assetsProvider = assetsProvider;
            _audioSystem = audioSystem;
        }

        public async UniTask Enter()
        {
            UIRoot uiRoot = await _uiRootFactory.GetUIRoot();
            uiRoot.HideLoadingScreen();
        }

        public async UniTask Exit()
        {
            _audioSystem.Stop();
            UIRoot uiRoot = await _uiRootFactory.GetUIRoot();
            uiRoot.ShowLoadingScreen();
            uiRoot.ClearSceneUI();
            _assetsProvider.CleanUp();
        }
    }
}