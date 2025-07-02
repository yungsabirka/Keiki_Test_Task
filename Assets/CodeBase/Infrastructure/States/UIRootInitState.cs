using CodeBase.Infrastructure.States.Core;
using CodeBase.Infrastructure.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace CodeBase.Infrastructure.States
{
    public class UIRootInitState : IState
    {
        private readonly IUIRootFactory _uiRootFactory;
        private readonly IStateMachine _gameStateMachine;

        public UIRootInitState(IStateMachine gameStateMachine, IUIRootFactory uiRootFactory)
        {
            _uiRootFactory = uiRootFactory;
            _gameStateMachine = gameStateMachine;
        }

        public async UniTask Enter()
        {
            UIRoot uiRoot = await _uiRootFactory.GetUIRoot();
            Object.DontDestroyOnLoad(uiRoot.gameObject);
            uiRoot.ShowLoadingScreen();
            await _gameStateMachine.Enter<ServicesInitializationState>();
        }

        public async UniTask Exit() {}
    }
}