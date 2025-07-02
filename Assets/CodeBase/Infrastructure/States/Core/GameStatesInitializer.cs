using CodeBase.Infrastructure.Scenes;
using CodeBase.Infrastructure.UI;
using CodeBase.Services.AssetsSystem;
using CodeBase.Services.AudioSystem;
using CodeBase.Services.AudioSystem.AudioSystem;
using CodeBase.Services.LevelsProvider;
using CodeBase.Services.ObjectsPool;
namespace CodeBase.Infrastructure.States.Core
{
    public class GameStatesInitializer : IGameStatesInitializer
    {
        private readonly IStateMachine _gameStateMachine;
        private readonly IUIRootFactory _uiRootFactory;
        private readonly IAssetsProvider _assetsProvider;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILevelsDataProvider _levelsDataProvider;
        private readonly IObjectsPool _objectsPool;
        private readonly IAudioSystem _audioSystem;

        public GameStatesInitializer(IStateMachine gameStateMachine, IUIRootFactory uiRootFactory, IAssetsProvider assetsProvider,
            ISceneLoader sceneLoader, ILevelsDataProvider levelsDataProvider, IObjectsPool objectsPool, IAudioSystem audioSystem)
        {
            _gameStateMachine = gameStateMachine;
            _uiRootFactory = uiRootFactory;
            _assetsProvider = assetsProvider;
            _sceneLoader = sceneLoader;
            _levelsDataProvider = levelsDataProvider;
            _objectsPool = objectsPool;
            _audioSystem = audioSystem;
        }

        public void CreateGameStates()
        {
            _gameStateMachine.AddState(new BootstrapState(_gameStateMachine, _sceneLoader));
            _gameStateMachine.AddState(new GameplayLoopState(_uiRootFactory, _objectsPool, _assetsProvider, _audioSystem));
            _gameStateMachine.AddState(new LoadGameplayState(_gameStateMachine, _sceneLoader));
            _gameStateMachine.AddState(new MainMenuLoopState(_uiRootFactory, _assetsProvider, _audioSystem));
            _gameStateMachine.AddState(new ServicesInitializationState(_gameStateMachine, _levelsDataProvider, _objectsPool, _audioSystem));
            _gameStateMachine.AddState(new UIRootInitState(_gameStateMachine, _uiRootFactory));
            _gameStateMachine.AddState(new LoadMainMenuState(_gameStateMachine, _sceneLoader));
        }
    }
}