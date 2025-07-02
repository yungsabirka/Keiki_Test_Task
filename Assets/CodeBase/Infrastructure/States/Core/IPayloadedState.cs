using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States.Core
{
    public interface IPayloadedState<TPayload> : IExitableState
    {
        UniTask Enter(TPayload enterParams);
    }
}