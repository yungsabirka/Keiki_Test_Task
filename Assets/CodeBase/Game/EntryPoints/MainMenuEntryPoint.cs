using CodeBase.Data.Levels;
using CodeBase.Infrastructure.UI;
using CodeBase.Services.LevelsProvider;
using CodeBase.UI.Screens.MainMenu;
using CodeBase.UI.Screens.MainMenu.Elements;
using CodeBase.UI.Screens.MainMenu.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
namespace CodeBase.Game.EntryPoints
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        private readonly UniTaskCompletionSource _initTaskCompletionSource = new();
        
        private IUIRootFactory _uiRootFactory;
        private MainMenuViewModel _mainMenuViewModel;
        private IMenuElementsFactory _menuElementsFactory;
        private ILevelsDataProvider _levelsDataProvider;
        
        [Inject]
        public void Construct(IUIRootFactory uiRootFactory, MainMenuViewModel mainMenuViewModel, IMenuElementsFactory menuElementsFactory, ILevelsDataProvider levelsDataProvider)
        {
            _uiRootFactory = uiRootFactory;
            _mainMenuViewModel = mainMenuViewModel;
            _menuElementsFactory = menuElementsFactory;
            _levelsDataProvider = levelsDataProvider;
        }
        
        private void Start() =>
            _initTaskCompletionSource.TrySetResult();
        
        public UniTask WaitForInitialization() =>
            _initTaskCompletionSource.Task;
        
        public async UniTask Run()
        {
            await _menuElementsFactory.Initialize();
            await InitializeScreenView();
            _mainMenuViewModel.Initialize();
        }

        private async UniTask InitializeScreenView()
        {
            MainMenuView mainMenuView = await _uiRootFactory.CreateScreen<MainMenuView>();
            var levelsData = _levelsDataProvider.LevelsData;
            foreach (LevelData levelData in levelsData)
            {
                HorizontalLevelsContainer levelContainer = _menuElementsFactory.CreateLevelContainer(levelData);
                mainMenuView.AddHorizontalLevelsContainer(levelContainer);
                for(var i = 0; i < levelData.LevelColors.Count; i++)
                {
                    LevelPageView unsetPageView = _menuElementsFactory.CreateUnsetLevelPageView();
                    mainMenuView.AddUnsetPage(unsetPageView);
                }
            }
            mainMenuView.Initialize(_mainMenuViewModel);
        }
    }
}