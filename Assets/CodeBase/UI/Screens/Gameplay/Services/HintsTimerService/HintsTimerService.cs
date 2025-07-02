using System;
using CodeBase.Services.Timer;
using R3;
using static CodeBase.Services.Timer.TimerConstants;
namespace CodeBase.UI.Screens.Gameplay.Services.HintsTimerService
{
    public class HintsTimerService : IHintsTimerService, IDisposable
    {
        private readonly ITimer _timer;

        public Subject<Unit> AudioHintRequested { get; } = new();
        public Subject<Unit> VisualHintRequested { get; } = new();
        public Subject<Unit> VisualHintHideRequested { get; } = new();
        
        public HintsTimerService(ITimer timer) =>
            _timer = timer;

        public void WaitForVisualHint()
        {
            VisualHintHideRequested.OnNext(Unit.Default);
            _timer.Start(TimeBeforeVisualHint, VisualHintRequested);
        }

        public void StopWaitingForVisualHint()
        {
            VisualHintHideRequested.OnNext(Unit.Default);
            _timer.Stop(VisualHintRequested);
        }

        public void WaitForAudioHint() => 
            _timer.Start(TimeBeforeAudioHint, AudioHintRequested);

        public void StopWaitingForAudioHint() =>
            _timer.Stop(AudioHintRequested);

        public void Dispose() =>
            _timer.StopAll();
    }
}