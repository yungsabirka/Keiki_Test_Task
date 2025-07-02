using Cysharp.Threading.Tasks;
using R3;
namespace CodeBase.Services.Timer
{
    public interface ITimer
    {
        UniTask Start(float seconds, Subject<Unit> onTimerComplete);

        void Stop(Subject<Unit> onTimerComplete);

        void StopAll();
    }
}