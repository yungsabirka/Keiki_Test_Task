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
        private readonly List<HintView> _hints = new();
        private readonly CompositeDisposable _disposables = new();

        private IHintsContainer _hintsContainer;
        private Sequence _showHintsSequence;

        public HintsPathService(IGameplayElementsFactory gameplayElementsFactory) =>
            _gameplayElementsFactory = gameplayElementsFactory;

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
            switch (data.FillType)
            {
                case FillType.Linear:
                    CreateLinearHints(data.StartPosition, data.EntPosition, data.ContourWidth);
                    break;
                case FillType.Radial:
                    CreateRadialHints(data.StartPosition, data.EntPosition, data.ContourWidth);
                    break;
            }
            ShowHints(showDuration);
        }

        private void ShowHints(float showDuration)
        {
            _showHintsSequence?.Kill();
            _showHintsSequence = DOTween.Sequence();
            float singleDuration = showDuration / _hints.Count;
            foreach(var hint in _hints)
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

        private void CreateLinearHints(Vector2 start, Vector2 end, float contourWidth)
        {
            float pathLength = Vector2.Distance(start, end);
            int hintCount = GetHintsCount(contourWidth, pathLength);

            for (int i = 0; i < hintCount; i++)
            {
                float t = (i + 1f) / (hintCount + 1f);
                Vector2 position = Vector2.Lerp(start, end, t);

                CreateHintView(contourWidth, i, hintCount, position);
            }
        }

        private void CreateRadialHints(Vector2 start, Vector2 end, float contourWidth)
        {
            Vector2 center = (start + end) / 2f;
            float radius = Vector2.Distance(start, end) / 2f;
            float circumference = 2 * Mathf.PI * radius;
            int hintCount = GetHintsCount(contourWidth, circumference);

            Vector2 radiusVector = start - center;
            float startAngle = Mathf.Atan2(radiusVector.y, radiusVector.x);

            for (int i = 0; i < hintCount; i++)
            {
                float angleOffset = -2f * Mathf.PI * (i / (float)hintCount);
                float angle = startAngle + angleOffset;

                Vector2 position = new Vector2(
                    center.x + radius * Mathf.Cos(angle),
                    center.y + radius * Mathf.Sin(angle)
                );

                CreateHintView(contourWidth, i, hintCount, position);
            }
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

        private int GetHintsCount(float contourWidth, float pathLength) =>
            Mathf.Max(1, Mathf.FloorToInt(pathLength / (2f * contourWidth * GetHintScale(HintType.Circle))));

        private float GetHintScale(HintType hintType) =>
            hintType == HintType.Circle
                ? 0.3f
                : 0.5f;

        private bool IsLastHint(int hintNumber, int hintCount) =>
            hintNumber == hintCount - 1;
    }
}