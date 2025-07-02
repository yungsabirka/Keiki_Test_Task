using System;
using CodeBase.UI.Screens.Gameplay.Services.FillPathSolver;
using R3;
using UnityEngine;
namespace CodeBase.UI.Popups.FingerHint
{
    public class FingerHintViewModel : IDisposable
    {
        private readonly FingerHintModel _model;
        private readonly CompositeDisposable _disposable = new();
        
        public Subject<Unit> VisualHintRequested { get; } = new();
        public Subject<Unit> VisualHintHideRequested { get; } = new();
        
        public FingerHintViewModel(FingerHintModel model) =>
            _model = model;

        public void Initialize()
        {
            SubscribeForVisualHintHideRequested();
            SubscribeForVisualHintRequested();
        }

        public void Dispose()
        {
            _model?.Dispose();
            _disposable?.Dispose();
        }
        
        public Vector2 GetPositionByFillAmount(FillType type, Vector2 startPosition, Vector2 endPosition, float fillAmount) =>
            _model.GetPositionByFillAmount(type, startPosition, endPosition, fillAmount);

        private void SubscribeForVisualHintHideRequested()
        {
            var visualHintHideSubscription = _model
                .VisualHintHideRequested
                .Subscribe(_ => OnVisualHintHideRequested());
            _disposable.Add(visualHintHideSubscription);
        }

        private void SubscribeForVisualHintRequested()
        {
            var visualHintRequestedSubscription = _model
                .VisualHintRequested
                .Subscribe(_ => OnVisualHintRequested());
            _disposable.Add(visualHintRequestedSubscription);
        }

        private void OnVisualHintRequested() =>
            VisualHintRequested.OnNext(Unit.Default);

        private void OnVisualHintHideRequested() =>
            VisualHintHideRequested.OnNext(Unit.Default);
    }
}