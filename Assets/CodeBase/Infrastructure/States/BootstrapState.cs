using CodeBase.Infrastructure.Scenes;
using CodeBase.Infrastructure.States.Core;
using Cysharp.Threading.Tasks;
using static CodeBase.Infrastructure.Scenes.SceneNames;
namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStateMachine _gameStateMachine;

        public BootstrapState(IStateMachine gameStateMachine, ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _gameStateMachine = gameStateMachine;
        }

        public async UniTask Enter() =>
            await _sceneLoader.LoadAsync(Boot, EnterUIRootInitState);

        public async UniTask Exit() {}

        private async UniTask EnterUIRootInitState() =>
            await _gameStateMachine.Enter<UIRootInitState>();
    }
}