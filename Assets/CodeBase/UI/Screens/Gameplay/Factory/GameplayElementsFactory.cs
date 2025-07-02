using System;
using CodeBase.Data.Levels;
using CodeBase.Services.AssetsSystem;
using CodeBase.Services.ObjectsPool;
using CodeBase.UI.Screens.Gameplay.Elements;
using CodeBase.UI.Screens.Gameplay.Elements.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static CodeBase.Services.AssetsSystem.Data.AssetsSystemConstants;
using Object = UnityEngine.Object;
namespace CodeBase.UI.Screens.Gameplay.Factory
{
    public class GameplayElementsFactory : IGameplayElementsFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IObjectsPool _objectsPool;

        private HintView _hintCirclePrefab;
        private HintView _hintStarPrefab;

        public GameplayElementsFactory(IAssetsProvider assetsProvider, IObjectsPool objectsPool)
        {
            _assetsProvider = assetsProvider;
            _objectsPool = objectsPool;
        }

        public async UniTask Initialize()
        {
            GameObject hintCircleGameObject = await _assetsProvider.LoadAsync<GameObject>(HintCircleViewKey);
            _hintCirclePrefab = hintCircleGameObject.GetComponent<HintView>();
            
            GameObject hintViewStarGameObject = await _assetsProvider.LoadAsync<GameObject>(HintStarViewKey);
            _hintStarPrefab = hintViewStarGameObject.GetComponent<HintView>();
        }

        public async UniTask<LevelConstructorView> CreateLevelConstructorView(LevelEnterParams enterParams)
        {
            if (enterParams.LevelType == LevelType.None)
                throw new ArgumentException("Cannot create level with None type");

            string levelPath = GetLevelPathByType(enterParams.LevelType);
            GameObject levelViewPrefab = await _assetsProvider.LoadAsync<GameObject>(levelPath);
            LevelConstructorView levelConstructorView = Object
                .Instantiate(levelViewPrefab)
                .GetComponent<LevelConstructorView>();

            return levelConstructorView;
        }

        public HintView CreateHintView(HintType hintType, float hintSize) =>
            hintType switch
            {
                HintType.Circle => CreateHintView(_hintCirclePrefab, hintSize),
                HintType.Star => CreateHintView(_hintStarPrefab, hintSize),
                _ => throw new ArgumentException("Invalid hint type")
            };

        private HintView CreateHintView(HintView prefab, float hintSize)
        {
            HintView hint = _objectsPool
                .GetObject(prefab)
                .GetComponent<HintView>();
            hint.Hide();
            hint.gameObject.SetActive(true);
            hint.SetSize(hintSize);
            return hint;
        }

        private string GetLevelPathByType(LevelType levelType) =>
            levelType switch
            {
                LevelType.Letter => LevelLetterKey,
                LevelType.Number => LevelNumberKey,
                LevelType.Shape => LevelShapeKey,
                _ => throw new ArgumentException("Cannot create level with invalid type")
            };
    }
}