using Cysharp.Threading.Tasks;
namespace CodeBase.Infrastructure.States.Core
{
    public interface IExitableState
    {
        UniTask Exit();
    }
}