using System;
using System.Threading;
using CodeBase.UI.Screens.Gameplay.Elements.Data;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
namespace CodeBase.UI.Screens.Gameplay
{
    public class GameplayViewModel : IDisposable
    {
        private readonly GameplayModel _model;
        private readonly CompositeDisposable _disposables = new();
        private readonly ReactiveProperty<LevelConstructorViewData> _currentLevelConstructorViewData = new();
        private readonly AsyncReactiveProperty<int> _completedLevelParts = new(0);
        private readonly ReactiveProperty<float> _currentFillAmount = new();
        private readonly ReactiveProperty<Vector2> _spaceShipPosition = new();
        private readonly CancellationTokenSource _completedLevelPartsCTS = new();
        
        private Tweener _fillAmountTween;

        public ReadOnlyAsyncReactiveProperty<int> CompletedLevelParts { get; private set; }
        public ReadOnlyReactiveProperty<LevelConstructorViewData> CurrentLevelConstructorViewData => _currentLevelConstructorViewData;
        public CancellationToken CompletedLevelPartsToken => _completedLevelPartsCTS.Token;
        public ReadOnlyReactiveProperty<float> CurrentFillAmount => _currentFillAmount;
        public ReactiveProperty<Vector2> SpaceShipPosition => _spaceShipPosition;
        public Subject<Unit> LevelEnded { get; } = new();

        public GameplayViewModel(GameplayModel model)
        {
            _model = model;
            CompletedLevelParts = new ReadOnlyAsyncReactiveProperty<int>(_completedLevelParts, _completedLevelPartsCTS.Token);
        }

        public void Initialize()
        {
            SubscribeForColorIndexChanged();
            SubscribeForFilledPartsCount();
            _disposables.Add(_model.LevelEnded.Subscribe(_ => OnLevelEnded()));
        }

        public void Dispose()
        {
            _completedLevelPartsCTS.Cancel();
            _completedLevelPartsCTS.Dispose();
            _disposables?.Dispose();
            _model.Dispose();
            _fillAmountTween?.Kill();
            _fillAmountTween = null;
        }

        public async UniTask CalculateNewFillAmount(InteractiveImagePathData pathData)
        {
            float fillAmount = _model.CalculateFillAmount(pathData);
            if (CanChangeFillAmount(fillAmount) == false)
                return;

            _currentFillAmount.Value = fillAmount < 0.99f
                ? fillAmount
                : 1f;

            _spaceShipPosition.Value = _model.GetPositionByFillAmount(pathData, fillAmount);

            if (CurrentImageFilled())
                await _model.CompleteLevelPart();
        }

        public void StartHintsTimer() =>
            _model.StartHintsTimer();

        public void StopHintsTimer() =>
            _model.StopHintsTimer();

        public void LoadNextLevel() =>
            _model.LoadNextLevel();

        public void LoadMainMenu() =>
            _model
                .LoadMainMenu()
                .Forget();

        public async UniTask PlayLevelStartAudioAsync() =>
            await _model.PlayLevelStartAudioAsync();

        private void SubscribeForColorIndexChanged()
        {
            var currentColorIndexSubscription = _model
                .CurrentLevelIndex
                .Skip(1)
                .Subscribe(index =>
                {
                    var currentLevelData = _model.GetCurrentLevelData();
                    _completedLevelParts.Value = 0;
                    _currentFillAmount.Value = 0f;
                    _currentLevelConstructorViewData.Value = new LevelConstructorViewData(currentLevelData.LevelColors[index]);
                });
            _disposables.Add(currentColorIndexSubscription);
        }

        private void SubscribeForFilledPartsCount()
        {
            var filledPartsCountSubscription = _model
                .CompletedLevelPartsCount
                .Skip(1)
                .Subscribe(filledPartsCount =>
                {
                    _completedLevelParts.Value = filledPartsCount;
                    _currentFillAmount.Value = 0f;
                });
            _disposables.Add(filledPartsCountSubscription);
        }

        private void OnLevelEnded() =>
            LevelEnded.OnNext(Unit.Default);

        private bool CurrentImageFilled() =>
            Mathf.Approximately(_currentFillAmount.Value, 1f);

        private bool CanChangeFillAmount(float fillAmount) =>
            fillAmount - _currentFillAmount.Value is > 0f and < 0.05f;
    }
}