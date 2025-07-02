using CodeBase.Infrastructure.States.Core;
using CodeBase.Services.AssetsSystem;
using CodeBase.Services.AudioSystem.AudioSystem;
using CodeBase.Services.LevelsProvider;
using CodeBase.Services.ObjectsPool;
using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States
{
    public class ServicesInitializationState : IState
    {
        private readonly IStateMachine _gameStateMachine;
        private readonly ILevelsDataProvider _levelsDataProvider;
        private readonly IObjectsPool _objectsPool;
        private readonly IAudioSystem _audioSystem;
        private readonly IAssetsProvider _assetsProvider;

        public ServicesInitializationState(IStateMachine gameStateMachine, ILevelsDataProvider levelsDataProvider, IObjectsPool objectsPool,
            IAudioSystem audioSystem, IAssetsProvider assetsProvider)
        {
            _gameStateMachine = gameStateMachine;
            _levelsDataProvider = levelsDataProvider;
            _objectsPool = objectsPool;
            _audioSystem = audioSystem;
            _assetsProvider = assetsProvider;
        }

        public async UniTask Enter()
        {
            await _assetsProvider.Initialize();
            await _levelsDataProvider.Initialize();
            _objectsPool.Initialize();
            await _audioSystem.Initialize();
            await _gameStateMachine.Enter<LoadMainMenuState>();
        }

        public async UniTask Exit() {}
    }
}