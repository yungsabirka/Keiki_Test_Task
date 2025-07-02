using System;
using System.Collections.Generic;
using CodeBase.UI.Screens.Gameplay.Elements;
using CodeBase.UI.Screens.Gameplay.Elements.Data;
using CodeBase.UI.Screens.Gameplay.Factory;
using CodeBase.UI.Screens.Gameplay.Services.FillPathSolver;
using DG.Tweening;
using R3;
using UnityEngine;
namespace CodeBase.UI.Screens.Gameplay.Services.HintsPath
{
    public class HintsPathService : IHintsPathService, IDisposable
    {
        private readonly IGameplayElementsFactory _gameplayElementsFactory;
        private readonly IFillPathSolver _fillPathSolver;
        private readonly List<HintView> _hints = new();
        private readonly CompositeDisposable _disposables = new();

        private IHintsContainer _hintsContainer;
        private Sequence _showHintsSequence;

        public HintsPathService(IGameplayElementsFactory gameplayElementsFactory, IFillPathSolver fillPathSolver)
        {
            _gameplayElementsFactory = gameplayElementsFactory;
            _fillPathSolver = fillPathSolver;
        }

        public void Initialize(IHintsContainer hintsContainer)
        {
            _hintsContainer = hintsContainer;
            _disposables
                .Add(_hintsContainer
                    .HintsRequested
                    .Subscribe(OnHintsRequested));
            _disposables
                .Add(_hintsContainer
                    .HintsHideRequested
                    .Subscribe(_ => ResetHints()));
            _disposables
                .Add(_hintsContainer
                    .Disabled
                    .Subscribe(_ => Dispose()));
        }

        public void Dispose()
        {
            _hints.Clear();
            _showHintsSequence?.Kill();
            _disposables?.Dispose();
        }

        private void OnHintsRequested(HintsPathData data)
        {
            const float showDuration = 1f;
            ResetHints();

            float distanceBetweenHints = 2f * data.ContourWidth * GetHintScale(HintType.Circle);
            List<Vector2> positions = _fillPathSolver
                .GetPath(data.FillType, data.StartPosition, data.EntPosition, distanceBetweenHints, 0f);

            for (int i = 0; i < positions.Count; ++i)
                CreateHintView(data.ContourWidth, i, positions.Count, positions[i]);

            ShowHints(showDuration);
        }

        private void ShowHints(float showDuration)
        {
            _showHintsSequence?.Kill();
            _showHintsSequence = DOTween.Sequence();
            float singleDuration = showDuration / _hints.Count;
            foreach (var hint in _hints)
                _showHintsSequence.Append(hint.Show(singleDuration));

            _showHintsSequence.Play();
        }

        private void ResetHints()
        {
            _showHintsSequence?.Kill(true);
            foreach (var hint in _hints)
                hint.ResetPoolable();
            _hints.Clear();
        }

        private void CreateHintView(float contourWidth, int hintNumber, int hintCount, Vector2 position)
        {
            HintType hintType = IsLastHint(hintNumber, hintCount)
                ? HintType.Star
                : HintType.Circle;

            float hintSize = contourWidth * GetHintScale(hintType);
            HintView hint = _gameplayElementsFactory.CreateHintView(hintType, hintSize);
            hint.transform.position = position;
            _hints.Add(hint);
            _hintsContainer.AddHintView(hint);
        }

        private float GetHintScale(HintType hintType) =>
            hintType == HintType.Circle
                ? 0.3f
                : 0.5f;

        private bool IsLastHint(int hintNumber, int hintCount) =>
            hintNumber == hintCount - 1;
    }
}