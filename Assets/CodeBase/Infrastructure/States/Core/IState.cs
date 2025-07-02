using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States.Core
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}