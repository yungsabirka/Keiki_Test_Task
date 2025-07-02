using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.UI;
using CodeBase.UI.Screens.MainMenu.Elements;
using CodeBase.UI.Screens.MainMenu.Elements.Data;
using ObservableCollections;
using R3;
using UnityEngine;
using UnityEngine.UI;
namespace CodeBase.UI.Screens.MainMenu
{
    public class MainMenuView : ScreenViewBase
    {
        [SerializeField] private Transform _verticalLevelsContainerContent;
        [SerializeField] private ScrollRect _verticalScrollRect;

        private readonly CompositeDisposable _disposables = new();
        private readonly List<LevelPageView> _unsetPages = new();
        private readonly List<LevelPageView> _setPages = new();
        private readonly List<HorizontalLevelsContainer> _horizontalLevelsContainers = new();

        private MainMenuViewModel _viewModel;

        private void OnDestroy() =>
            _disposables?.Dispose();

        public void Initialize(MainMenuViewModel viewModel)
        {
            _viewModel = viewModel;
            SubscribeToPageViewDataAdded();
        }

        public void AddHorizontalLevelsContainer(HorizontalLevelsContainer horizontalLevelsContainer)
        {
            horizontalLevelsContainer.transform.SetParent(_verticalLevelsContainerContent);
            horizontalLevelsContainer.SetParentScroll(_verticalScrollRect);
            _horizontalLevelsContainers.Add(horizontalLevelsContainer);
        }

        public void AddUnsetPage(LevelPageView unsetPage)
        {
            _unsetPages.Add(unsetPage);
            unsetPage.gameObject.SetActive(false);
        }

        private void OnPageViewDataAdded(LevelPageViewData viewData)
        {
            HorizontalLevelsContainer levelsContainer = _horizontalLevelsContainers.First(container => container.LevelType == viewData.LevelType);
            LevelPageView page = _unsetPages.First();
            _unsetPages.Remove(page);
            page.Initialize(viewData.Sprite, viewData.Color, viewData.LevelType, viewData.AspectRatio);
            _setPages.Add(page);
            SubscribeToLevelPageClicked(page);
            levelsContainer.AddLevelPage(page);
        }

        private void SubscribeToPageViewDataAdded()
        {
            var pageViewDataAddSubscription = _viewModel
                .PagesViewData
                .ObserveAdd()
                .Subscribe(pageViewData => OnPageViewDataAdded(pageViewData.Value));
            _disposables.Add(pageViewDataAddSubscription);
        }

        private void SubscribeToLevelPageClicked(LevelPageView levelPage)
        {
            var pageClickedSubscription = levelPage
                .LevelPageClicked
                .Subscribe(page => _viewModel
                    .OpenLevel(page.LevelType, page.PageColor));

            _disposables.Add(pageClickedSubscription);
        }
    }
}