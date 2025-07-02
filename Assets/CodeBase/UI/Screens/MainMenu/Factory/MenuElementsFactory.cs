using System;
using CodeBase.Data.Levels;
using CodeBase.Services.AssetsSystem;
using CodeBase.UI.Screens.MainMenu.Elements;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static CodeBase.Services.AssetsSystem.Data.AssetsSystemConstants;
using Object = UnityEngine.Object;
namespace CodeBase.UI.Screens.MainMenu.Factory
{
    public class MenuElementsFactory : IMenuElementsFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private GameObject _levelsContainerPrefab;
        private GameObject _levelPageViewPrefab;

        public MenuElementsFactory(IAssetsProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public async UniTask Initialize()
        {
            _levelsContainerPrefab = await _assetsProvider
                .LoadAsync<GameObject>(HorizontalLevelsContainerKey);
            _levelPageViewPrefab = await _assetsProvider
                .LoadAsync<GameObject>(LevelPageKey);
        }

        public HorizontalLevelsContainer CreateLevelContainer(LevelData levelData)
        {
            HorizontalLevelsContainer levelsContainer = Object
                .Instantiate(_levelsContainerPrefab)
                .GetComponent<HorizontalLevelsContainer>();

            var levelTitle = GetLevelsTitle(levelData.LevelType);
            levelsContainer.SetLevelsTitle(levelTitle);
            levelsContainer.SetLevelType(levelData.LevelType);
            return levelsContainer;
        }

        public LevelPageView CreateUnsetLevelPageView()
        {
            LevelPageView unsetPage = Object
                .Instantiate(_levelPageViewPrefab)
                .GetComponent<LevelPageView>();
            
            return unsetPage;
        }

        private string GetLevelsTitle(LevelType levelType)
        {
            if (levelType == LevelType.None)
                throw new ArgumentException("Invalid level type");
            return $"Trace {levelType.ToString()}";
        }
    }
}