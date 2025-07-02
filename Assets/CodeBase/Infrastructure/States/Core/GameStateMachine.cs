using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States.Core
{
    public class GameStateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states = new();
        
        private IExitableState _activeState;

        public void AddState<TState>(TState state) where TState : class, IExitableState
        {
            if(_states.ContainsKey(state.GetType()))
                throw new ArgumentException("State already added");
            
            _states.Add(state.GetType(), state);
        }

        public async UniTask Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            await state.Enter();
        }

        public async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            await state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}