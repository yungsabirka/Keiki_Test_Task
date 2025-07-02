using System;
using System.Collections.Generic;
using UnityEngine;
namespace CodeBase.Infrastructure.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private Transform _layerHUD;
        [SerializeField] private Transform _layerPopups;
        [SerializeField] private Transform _loadingScreen;

        private List<Transform> _uiLayers;

        private void Awake()
        {
            _uiLayers = new List<Transform>
            {
                _layerHUD,
                _layerPopups,
            };
        }

        public void AttachSceneUI(GameObject uiObject, UILayer layer)
        {
            var parent = GetLayerTransform(layer);
            if (parent == null)
                throw new Exception("UI attachment failed: the specified parent layer is null or invalid.");

            uiObject.transform.SetParent(parent, false);
        }

        public void ClearSceneUI()
        {
            foreach (var layer in _uiLayers)
            foreach (Transform child in layer)
                Destroy(child.gameObject);
        }

        public void ShowLoadingScreen() =>
            _loadingScreen.gameObject.SetActive(true);
        
        public void HideLoadingScreen() => 
            _loadingScreen.gameObject.SetActive(false);

        private Transform GetLayerTransform(UILayer layer)
        {
            return layer switch
            {
                UILayer.HUD => _layerHUD,
                UILayer.Popups => _layerPopups,
                _ => null
            };
        }
    }
}