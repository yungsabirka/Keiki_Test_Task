using CodeBase.Data.Levels;
using CodeBase.Infrastructure.UI;
using CodeBase.UI.Popups.FingerHint;
using CodeBase.UI.Screens.Gameplay;
using CodeBase.UI.Screens.Gameplay.Elements;
using CodeBase.UI.Screens.Gameplay.Factory;
using CodeBase.UI.Screens.Gameplay.Services.HintsPath;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
namespace CodeBase.Game.EntryPoints
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private readonly UniTaskCompletionSource _initTaskCompletionSource = new();
        
        private GameplayModel _gameplayModel;
        private GameplayViewModel _gameplayViewModel;
        private IUIRootFactory _uiRootFactory;
        private IGameplayElementsFactory _gameplayElementsFactory;
        private IHintsPathService _hintsPathService;
        private FingerHintModel _fingerHintModel;
        private FingerHintViewModel _fingerHintViewModel;
        private LevelEnterParams _enterParams;

        [Inject]
        public void Construct(IUIRootFactory uiRootFactory, GameplayModel gameplayModel, GameplayViewModel gameplayViewModel, 
            IGameplayElementsFactory gameplayElementsFactory, IHintsPathService hintsPathService, FingerHintModel fingerHintModel,
            FingerHintViewModel fingerHintViewModel)
        {
            _uiRootFactory = uiRootFactory;
            _gameplayModel = gameplayModel;
            _gameplayViewModel = gameplayViewModel;
            _gameplayElementsFactory = gameplayElementsFactory;
            _hintsPathService = hintsPathService;
            _fingerHintModel = fingerHintModel;
            _fingerHintViewModel = fingerHintViewModel;
        }
        
        private void Start() =>
            _initTaskCompletionSource.TrySetResult();
        
        public UniTask WaitForInitialization() =>
            _initTaskCompletionSource.Task;
        
        public async UniTask Run(LevelEnterParams enterParams)
        {
            _enterParams = enterParams;
            await _gameplayElementsFactory.Initialize();
            LevelConstructorView levelConstructorView = await _gameplayElementsFactory.CreateLevelConstructorView(enterParams);
            await InitializeFingerPopup(levelConstructorView);
            await InitializeGameplayScreenView(enterParams, levelConstructorView);
        }
        
        public void StartGame() =>
            _gameplayModel.StartGame(_enterParams);

        private async UniTask InitializeGameplayScreenView(LevelEnterParams enterParams, LevelConstructorView levelConstructorView)
        {
            _gameplayModel.Initialize();
            _gameplayViewModel.Initialize();
            GameplayView gameplayView = await _uiRootFactory.CreateScreen<GameplayView>();
            _hintsPathService.Initialize(levelConstructorView);
            gameplayView.AddLevelConstructorView(levelConstructorView);
            gameplayView.Initialize(_gameplayViewModel);
        }

        private async UniTask InitializeFingerPopup(LevelConstructorView levelConstructorView)
        {
            FingerHintView fingerHintView = await _uiRootFactory.CreatePopup<FingerHintView>();
            _fingerHintModel.Initialize();
            _fingerHintViewModel.Initialize();
            fingerHintView.Initialize(_fingerHintViewModel, levelConstructorView);
        }
    }
}