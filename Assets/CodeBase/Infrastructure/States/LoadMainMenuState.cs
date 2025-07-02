using CodeBase.Game.EntryPoints;
using CodeBase.Infrastructure.Scenes;
using CodeBase.Infrastructure.States.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static CodeBase.Infrastructure.Scenes.SceneNames;
namespace CodeBase.Infrastructure.States
{
    public class LoadMainMenuState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStateMachine _gameStateMachine;

        public LoadMainMenuState(IStateMachine gameStateMachine, ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _gameStateMachine = gameStateMachine;
        }

        public async UniTask Enter()
        {
            await _sceneLoader.LoadAsync(MainMenu);
            
            await UniTask.Yield();

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            await sceneEntryPoint.WaitForInitialization();
            await sceneEntryPoint.Run();
            await _gameStateMachine.Enter<MainMenuLoopState>();
        }

        public async UniTask Exit() {}
    }
}