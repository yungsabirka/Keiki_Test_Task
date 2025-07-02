using System;
using System.Collections.Generic;
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
        
        public List<Vector2> GetAnimationPath(FillType type, Vector2 start, Vector2 end, float distanceBetweenPoints, float fillAmount) =>
            _fillPathSolver.GetPath(type, start, end, distanceBetweenPoints, fillAmount);

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