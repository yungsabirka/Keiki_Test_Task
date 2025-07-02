using R3;
namespace CodeBase.UI.Screens.Gameplay.Services.HintsTimerService
{
    public interface IHintsTimerService
    {
        Subject<Unit> AudioHintRequested { get; }
        Subject<Unit> VisualHintRequested { get; }
        Subject<Unit> VisualHintHideRequested { get; }

        void WaitForAudioHint();

        void WaitForVisualHint();

        void StopWaitingForVisualHint();

        void StopWaitingForAudioHint();
    }
}