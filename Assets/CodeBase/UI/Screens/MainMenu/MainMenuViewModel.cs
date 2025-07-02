using CodeBase.Data.Levels;
using CodeBase.UI.Screens.MainMenu.Elements.Data;
using Cysharp.Threading.Tasks;
using ObservableCollections;
using UnityEngine;
namespace CodeBase.UI.Screens.MainMenu
{
    public class MainMenuViewModel
    {
        private readonly MainMenuModel _model;
        private readonly ObservableList<LevelPageViewData> _pagesViewData = new();
        
        public IReadOnlyObservableList<LevelPageViewData> PagesViewData => _pagesViewData;

        public MainMenuViewModel(MainMenuModel model)
        {
            _model = model;
        }

        public void Initialize()
        {
            CreateLevelPagesViewData();
        }

        private void CreateLevelPagesViewData()
        {
            foreach (LevelData levelData in _model.LevelsData)
            foreach (Color color in levelData.LevelColors)
            {
                var spriteAspectRatio = levelData.LevelSprite.rect.width / levelData.LevelSprite.rect.height;
                _pagesViewData.Add(new LevelPageViewData(levelData.LevelType, levelData.LevelSprite, color, spriteAspectRatio));
            }
        }

        public void OpenLevel(LevelType levelType, Color color)
        {
            _model
                .LoadGameplay(levelType, color)
                .Forget();
        }
    }
}