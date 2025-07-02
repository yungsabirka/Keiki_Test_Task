using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States.Core
{
    public interface IStateMachine
    {
        void AddState<TState>(TState state) where TState : class, IExitableState;
        UniTask Enter<TState>() where TState : class, IState;
        UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
    }
}