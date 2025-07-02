using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
namespace CodeBase.Services.Timer
{
    public class Timer : ITimer
    {
        private readonly Dictionary<Subject<Unit>, CancellationTokenSource> _activeTimers = new();

        public async UniTask Start(float seconds, Subject<Unit> onTimerComplete)
        {
            if (_activeTimers.TryGetValue(onTimerComplete, out var existingCts))
            {
                existingCts.Cancel();
                existingCts.Dispose();
                _activeTimers.Remove(onTimerComplete);
            }

            var cts = new CancellationTokenSource();
            _activeTimers[onTimerComplete] = cts;

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: cts.Token);
                if (!cts.IsCancellationRequested)
                {
                    onTimerComplete.OnNext(Unit.Default);
                    _activeTimers.Remove(onTimerComplete);
                }
            }
            catch (OperationCanceledException) {}
        }
        
        public void Stop(Subject<Unit> onTimerComplete)
        {
            if (_activeTimers.TryGetValue(onTimerComplete, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
                _activeTimers.Remove(onTimerComplete);
            }
        }

        public void StopAll()
        {
            foreach (var kv in _activeTimers)
            {
                kv.Value.Cancel();
                kv.Value.Dispose();
            }

            _activeTimers.Clear();
        }
    }
}