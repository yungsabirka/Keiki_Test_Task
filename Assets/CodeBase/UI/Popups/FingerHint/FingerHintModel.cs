using System;
using CodeBase.UI.Screens.Gameplay.Services.FillPathSolver;
using CodeBase.UI.Screens.Gameplay.Services.HintsTimerService;
using R3;
using UnityEngine;
namespace CodeBase.UI.Popups.FingerHint
{
    public class FingerHintModel : IDisposable
    {
        private readonly IHintsTimerService _hintsTimerService;
        private readonly IFillPathSolver _fillPathSolver;
        private readonly CompositeDisposable _disposable = new();

        public Subject<Unit> VisualHintRequested { get; } = new();
        public Subject<Unit> VisualHintHideRequested { get; } = new();
        
        public FingerHintModel(IHintsTimerService hintsTimerService, IFillPathSolver fillPathSolver)
        {
            _hintsTimerService = hintsTimerService;
            _fillPathSolver = fillPathSolver;
        }

        public void Initialize()
        {
            SubscribeForVisualHintRequested();
            SubscribeForVisualHintTimerStarted();
        }
        
        public Vector2 GetPositionByFillAmount(FillType type, Vector2 startPosition, Vector2 endPosition, float fillAmount) =>
            _fillPathSolver.GetPositionByFillAmount(type, startPosition, endPosition, fillAmount);
        
        public void Dispose() =>
            _disposable?.Dispose();

        private void SubscribeForVisualHintTimerStarted()
        {
            var visualHintHideRequested = _hintsTimerService
                .VisualHintHideRequested
                .Subscribe(_ => OnVisualHintHideRequested());
            _disposable.Add(visualHintHideRequested);
        }

        private void SubscribeForVisualHintRequested()
        {
            var visualHintRequestedSubscription = _hintsTimerService
                .VisualHintRequested
                .Subscribe(_ => OnVisualHintRequested());
            _disposable.Add(visualHintRequestedSubscription);
        }

        private void OnVisualHintHideRequested() =>
            VisualHintHideRequested.OnNext(Unit.Default);

        private void OnVisualHintRequested() =>
            VisualHintRequested.OnNext(Unit.Default);
    }
}