using CodeBase.Data.Levels;
using CodeBase.Infrastructure.UI;
using CodeBase.UI.Screens.Gameplay.Elements;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using R3;
using UnityEngine;
using UnityEngine.UI;
namespace CodeBase.UI.Screens.Gameplay
{
    public class GameplayView : ScreenViewBase
    {
        [SerializeField] private Button _homeButton;

        private readonly CompositeDisposable _disposables = new();

        private GameplayViewModel _viewModel;
        private LevelConstructorView _levelConstructorView;

        private void OnDestroy()
        {
            _homeButton.onClick.RemoveAllListeners();
            _disposables?.Dispose();
            _viewModel.Dispose();
        }

        public void Initialize(GameplayViewModel viewModel)
        {
            _viewModel = viewModel;
            SubscribeForHomeButton();
            SubscribeForLevelViewDataChanged();
            SubscribeForCompletedLevelPartsCount();
            SubscribeForFillAmountChanged();
            SubscribeForLevelEnded();
            SubscribeForSpaceShipPositionChanged();
            SubscribeForSpaceShipDrag();
        }

        private void SubscribeForSpaceShipPositionChanged()
        {
            var spaceShipPositionSubscription = _viewModel
                .SpaceShipPosition
                .Skip(1)
                .Subscribe(OnSpaceShipPositionChanged);
            _disposables.Add(spaceShipPositionSubscription);
        }

        private void OnSpaceShipPositionChanged(Vector2 position) =>
            _levelConstructorView.SetSpaceShipPosition(position);

        private void SubscribeForLevelEnded()
        {
            var levelEndedSubscription = _viewModel
                .LevelEnded
                .Subscribe(_ => OnLevelEnded());
            _disposables.Add(levelEndedSubscription);
        }

        private void SubscribeForCompletedLevelPartsCount()
        {
            var completedLevelPartsSubscription = _viewModel
                .CompletedLevelParts
                .Skip(1)
                .SubscribeAwait(async completedLevelPartsCount => await InitializeNextLevelPart(completedLevelPartsCount));
            _disposables.Add(completedLevelPartsSubscription);
        }

        private void SubscribeForSpaceShipDrag()
        {
            var spaceShipDragStartedSubscription = _levelConstructorView
                .SpaceShipDragStarted
                .Subscribe(_ => OnSpaceShipDragStarted());
            _disposables.Add(spaceShipDragStartedSubscription);
            var spaceShipDragEndedSubscription = _levelConstructorView
                .SpaceShipDragEnded
                .Subscribe(_ => OnSpaceShipDragEnded());
            _disposables.Add(spaceShipDragEndedSubscription);
        }

        private async UniTask InitializeNextLevelPart(int completedLevelPartsCount)
        {
            _levelConstructorView.HideHints();
            for (int i = 0; i < _levelConstructorView.InteractiveFillImages.Count; i++)
            {
                var filledImage = _levelConstructorView.InteractiveFillImages[i];
                if (completedLevelPartsCount == i)
                {
                    filledImage.SetActive(true);
                    _levelConstructorView.MoveSpaceShipToPosition(filledImage.GetStartPosition());
                    _levelConstructorView.SetActiveFillImage(filledImage);

                    if (completedLevelPartsCount == 0)
                        await _viewModel.PlayLevelStartAudioAsync();

                    _levelConstructorView.ShowHints();
                }
                else if (completedLevelPartsCount < i)
                    filledImage.SetActive(false);
            }
            _viewModel.StartHintsTimer();
        }

        private void SubscribeForLevelViewDataChanged()
        {
            var levelViewDataChanged = _viewModel
                .CurrentLevelConstructorViewData
                .Skip(1)
                .Subscribe(constructorViewData => _levelConstructorView.Initialize(constructorViewData.LevelColor));
            _disposables.Add(levelViewDataChanged);
        }

        private void SubscribeForFillAmountChanged()
        {
            var fillAmountSubscription = _viewModel
                .CurrentFillAmount
                .Skip(1)
                .Subscribe(fillAmount =>
                {
                    _levelConstructorView.ActiveFillImage.FillImage(fillAmount);
                });
            _disposables.Add(fillAmountSubscription);
        }

        public void AddLevelConstructorView(LevelConstructorView levelConstructorView)
        {
            _levelConstructorView = levelConstructorView;
            _levelConstructorView.transform.SetParent(transform, false);
            var spaceShipDraggingSubscription = _levelConstructorView
                .SpaceShipDragging
                .Subscribe(OnSpaceShipDragging);
            _disposables.Add(spaceShipDraggingSubscription);
        }

        private void OnSpaceShipDragging(InteractiveImagePathData pathData) =>
            _viewModel
                .CalculateNewFillAmount(pathData)
                .Forget();

        private void OnSpaceShipDragStarted() =>
            _viewModel.StopHintsTimer();

        private void OnSpaceShipDragEnded() =>
            _viewModel.StartHintsTimer();

        private void SubscribeForHomeButton() =>
            _homeButton.onClick.AddListener(() => _viewModel.LoadMainMenu());

        private void OnLevelEnded() =>
            _viewModel.LoadNextLevel();
    }
}