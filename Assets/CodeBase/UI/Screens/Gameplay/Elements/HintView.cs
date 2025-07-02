using System;
using CodeBase.Services.ObjectsPool;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace CodeBase.UI.Screens.Gameplay.Elements
{
    public class HintView : MonoBehaviour, IPoolable
    {
        [SerializeField] private Image _hintImage;

        private Tweener _showTween;

        public GameObject GameObject => gameObject;

        public event Action<IPoolable> ReturnRequested;

        public void ResetPoolable() =>
            ReturnRequested?.Invoke(this);

        private void OnDisable()
        {
            _showTween?.Kill();
            _showTween = null;
        }

        public Tweener Show(float duration)
        {
            _showTween?.Kill();
            return _showTween = _hintImage.DOColor(new Color(255, 255, 255, 0.9f), duration);
        }

        public void Hide()
        {
            _showTween?.Kill();
            _hintImage.color = new Color(255, 255, 255, 0);
        }

        public void SetSize(float size) =>
            _hintImage.rectTransform.sizeDelta = new Vector2(size, size);
    }
}