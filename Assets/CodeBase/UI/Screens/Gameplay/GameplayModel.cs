using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data.Levels;
using CodeBase.Infrastructure.States;
using CodeBase.Infrastructure.States.Core;
using CodeBase.Services.LevelsProvider;
using CodeBase.UI.Screens.Gameplay.Services.AudioService;
using CodeBase.UI.Screens.Gameplay.Services.FillPathSolver;
using CodeBase.UI.Screens.Gameplay.Services.HintsTimerService;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
namespace CodeBase.UI.Screens.Gameplay
{
    public class GameplayModel : IDisposable
    {
        private readonly IStateMachine _gameStateMachine;
        private readonly ILevelsDataProvider _levelsDataProvider;
        private readonly IFillPathSolver _fillPathSolver;
        private readonly IHintsTimerService _hintsTimerService;
        private readonly IGameplayAudioService _gameplayAudioService;
        private readonly ReactiveProperty<int> _currentLevelIndex = new(0);
        private readonly ReactiveProperty<int> _completedLevelPartsCount = new();
        private readonly CompositeDisposable _disposables = new();

        public ReadOnlyReactiveProperty<int> CompletedLevelPartsCount => _completedLevelPartsCount;
        public ReadOnlyReactiveProperty<int> CurrentLevelIndex => _currentLevelIndex;
        public Subject<Unit> LevelEnded { get; } = new();
        public LevelType LevelType { get; private set; }

        public GameplayModel(IStateMachine gameStateMachine, ILevelsDataProvider levelsDataProvider, IFillPathSolver fillPathSolver,
            IHintsTimerService hintsTimerService, IGameplayAudioService gameplayAudioService)
        {
            _gameStateMachine = gameStateMachine;
            _levelsDataProvider = levelsDataProvider;
            _fillPathSolver = fillPathSolver;
            _hintsTimerService = hintsTimerService;
            _gameplayAudioService = gameplayAudioService;
        }

        public void Initialize()
        {
            _disposables
                .Add(_hintsTimerService.AudioHintRequested
                    .Subscribe(_ => OnAudioHintRequested()));
        }

        public void StartGame(LevelEnterParams enterParams)
        {
            LevelType = enterParams.LevelType;
            var currentLevelData = GetCurrentLevelData();
            _currentLevelIndex.Value = currentLevelData.LevelColors.IndexOf(enterParams.LevelColor);
        }

        public async UniTask CompleteLevelPart()
        {
            await _gameplayAudioService.PlayRandomRewardAudioAsync();
            _completedLevelPartsCount.Value++;

            if (_completedLevelPartsCount.Value != GetCurrentLevelData()
                .FilledPartsCount)
                return;

            LevelEnded.OnNext(Unit.Default);
        }

        public void StartHintsTimer()
        {
            _hintsTimerService.WaitForAudioHint();
            _hintsTimerService.WaitForVisualHint();
        }

        public void StopHintsTimer()
        {
            _hintsTimerService.StopWaitingForAudioHint();
            _hintsTimerService.StopWaitingForVisualHint();
        }

        public async UniTask PlayLevelStartAudioAsync() =>
            await _gameplayAudioService.PlayLevelStartAudioAsync(LevelType);

        public float CalculateFillAmount(InteractiveImagePathData pathData) =>
            _fillPathSolver.CalculateFillAmount(pathData.FillType, pathData.StartPoint,
                pathData.EndPoint, pathData.InputPoint);

        public Vector2 GetPositionByFillAmount(InteractiveImagePathData pathData, float fillAmount) =>
            _fillPathSolver.GetPositionByFillAmount(pathData.FillType, pathData.StartPoint,
                pathData.EndPoint, fillAmount);

        public void LoadNextLevel()
        {
            var currentLevelData = GetCurrentLevelData();

            if (currentLevelData == null)
                throw new Exception("Cannot find current level data with type " + LevelType);

            _currentLevelIndex.Value = (_currentLevelIndex.Value + 1) % currentLevelData.LevelColors.Count;
            _completedLevelPartsCount.Value = 0;
        }

        public LevelData GetCurrentLevelData() =>
            _levelsDataProvider
                .LevelsData
                .FirstOrDefault(levelData => levelData.LevelType == LevelType);

        public async UniTask LoadMainMenu() =>
            await _gameStateMachine.Enter<LoadMainMenuState>();

        public void Dispose() =>
            _disposables?.Dispose();

        private void OnAudioHintRequested() =>
            _gameplayAudioService.PlayLevelStartAudioAsync(LevelType);
    }
}