using System;
using System.Collections.Generic;
using CodeBase.Services.AssetsSystem;
using CodeBase.UI.Popups.FingerHint;
using CodeBase.UI.Screens.Gameplay;
using CodeBase.UI.Screens.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static CodeBase.Services.AssetsSystem.Data.AssetsSystemConstants;
using Object = UnityEngine.Object;
namespace CodeBase.Infrastructure.UI
{
    public class UIRootFactory : IUIRootFactory
    {
        private readonly Dictionary<Type, string> _popupPrefabKeys;
        private readonly Dictionary<Type, string> _screenPrefabKeys;
        private readonly IAssetsProvider _assetsProvider;
        private UIRoot _uiRoot;

        public UIRootFactory(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;

            _popupPrefabKeys = new Dictionary<Type, string>
            {
                {typeof(FingerHintView), FingerHintPopupKey}
            };

            _screenPrefabKeys = new Dictionary<Type, string>
            {
                {typeof(MainMenuView), MainMenuScreenKey},
                {typeof(GameplayView), GameplayScreenKey}
            };
        }

        public async UniTask<UIRoot> GetUIRoot()
        {
            if (IsUIRootCreated())
                return _uiRoot;

            var uiRootPrefab = await _assetsProvider.LoadAsync<GameObject>(UIRootPrefabKey);
            _uiRoot = Object
                .Instantiate(uiRootPrefab)
                .GetComponent<UIRoot>();
            return _uiRoot;
        }

        public async UniTask<TScreen> CreateScreen<TScreen>() where TScreen : ScreenViewBase
        {
            CheckUIRootCreation();

            string screenViewPrefabKey = GetScreenViewPrefabKey<TScreen>();
            GameObject screenViewPrefab = await _assetsProvider.LoadAsync<GameObject>(screenViewPrefabKey);
            TScreen screenView = Object
                .Instantiate(screenViewPrefab)
                .GetComponent<TScreen>();

            _uiRoot.AttachSceneUI(screenView.gameObject, UILayer.HUD);
            return screenView;
        }

        public async UniTask<TPopup> CreatePopup<TPopup>() where TPopup : PopupViewBase
        {
            CheckUIRootCreation();

            string popupPrefabKey = GetPopupPrefabKey<TPopup>();
            GameObject popupPrefab = await _assetsProvider.LoadAsync<GameObject>(popupPrefabKey);
            TPopup popupView = Object
                .Instantiate(popupPrefab)
                .GetComponent<TPopup>();

            _uiRoot.AttachSceneUI(popupView.gameObject, UILayer.Popups);
            return popupView;
        }

        private string GetPopupPrefabKey<TPopup>() where TPopup : PopupViewBase
        {
            var popupType = typeof(TPopup);
            if (_popupPrefabKeys.TryGetValue(popupType, out var key))
                return key;

            throw new Exception($"Unknown popup view type: {popupType.Name}");
        }

        private string GetScreenViewPrefabKey<TScreen>() where TScreen : ScreenViewBase
        {
            var screenType = typeof(TScreen);
            if (_screenPrefabKeys.TryGetValue(screenType, out var key))
                return key;

            throw new Exception($"Unknown screen view type: {screenType.Name}");
        }

        private void CheckUIRootCreation()
        {
            if (IsUIRootCreated() == false)
                throw new Exception("Cannot create the main menu screen without creating a UIRoot.");
        }

        private bool IsUIRootCreated() =>
            _uiRoot != null;
    }
}