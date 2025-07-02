using System;
using CodeBase.Infrastructure.States;
using CodeBase.Infrastructure.States.Core;
using UnityEngine;
using Zenject;
namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private IStateMachine _stateMachine;
        private IGameStatesInitializer _gameStatesInitializer;

        [Inject]
        public void Construct(IStateMachine stateMachine, IGameStatesInitializer gameStatesInitializer)
        {
            _stateMachine = stateMachine;
            _gameStatesInitializer = gameStatesInitializer;
        }

        private async void Start()
        {
            try
            {
                Application.targetFrameRate = 60;
                
                DontDestroyOnLoad(gameObject);
                _gameStatesInitializer.CreateGameStates();
                await _stateMachine.Enter<BootstrapState>();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}