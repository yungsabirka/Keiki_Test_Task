using System;
using CodeBase.Data.Levels;
using CodeBase.Game.EntryPoints;
using CodeBase.Infrastructure.Scenes;
using CodeBase.Infrastructure.States.Core;
using Cysharp.Threading.Tasks;
using static CodeBase.Infrastructure.Scenes.SceneNames;
using Object = UnityEngine.Object;
namespace CodeBase.Infrastructure.States
{
    public class LoadGameplayState : IPayloadedState<LevelEnterParams>
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStateMachine _gameStateMachine;

        public LoadGameplayState(IStateMachine gameStateMachine, ISceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public async UniTask Enter(LevelEnterParams enterParams)
        {
            await _sceneLoader.LoadAsync(Gameplay);
            
            await UniTask.Yield();

            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            await sceneEntryPoint.WaitForInitialization();
            await sceneEntryPoint.Run(enterParams);
            await _gameStateMachine.Enter<GameplayLoopState, Action>(sceneEntryPoint.StartGame);
        }

        public async UniTask Exit()
        {
        }
    }
}