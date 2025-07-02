using System;
using System.Collections.Generic;
using CodeBase.UI.Popups.FingerHint.Data;
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
        public Subject<FingerAnimationPathData> AnimationPathReady { get; } = new();
        
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
        
        public void PrepareAnimationPath(FillType type, Vector2 start, Vector2 end, float distanceBetweenPoints, float fillAmount)
        {
            List<Vector2> positions = _model.GetAnimationPath(type, start, end, distanceBetweenPoints, fillAmount);
            float pathLength = 0f;
            
            for (int i = 1; i < positions.Count; i++)
                pathLength += Vector2.Distance(positions[i - 1], positions[i]);
            
            AnimationPathReady.OnNext(new FingerAnimationPathData(positions, pathLength));
        }

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