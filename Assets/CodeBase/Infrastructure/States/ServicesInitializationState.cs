using CodeBase.Infrastructure.States.Core;
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

        public ServicesInitializationState(IStateMachine gameStateMachine, ILevelsDataProvider levelsDataProvider, IObjectsPool objectsPool, IAudioSystem audioSystem)
        {
            _gameStateMachine = gameStateMachine;
            _levelsDataProvider = levelsDataProvider;
            _objectsPool = objectsPool;
            _audioSystem = audioSystem;
        }

        public async UniTask Enter()
        {
            await _levelsDataProvider.Initialize();
            _objectsPool.Initialize();
            _audioSystem.Initialize();
            await _gameStateMachine.Enter<LoadMainMenuState>();
        }

        public async UniTask Exit() {}
    }
}