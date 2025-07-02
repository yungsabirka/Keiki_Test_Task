using CodeBase.Data.Levels;
using R3;
using UnityEngine;
using UnityEngine.UI;
namespace CodeBase.UI.Screens.MainMenu.Elements
{
    public class LevelPageView : MonoBehaviour
    {
        [SerializeField] private Image _levelImage;
        [SerializeField] private Button _button;

        public readonly Subject<LevelPageView> LevelPageClicked = new();
        public Color PageColor => _levelImage.color;
        public LevelType LevelType { get; private set; }

        private void OnEnable() =>
            _button.onClick.AddListener(() => LevelPageClicked?.OnNext(this));

        private void OnDisable() =>
            _button.onClick.RemoveAllListeners();

        public void Initialize(Sprite levelSprite, Color levelColor, LevelType levelType, float aspectRatio)
        {
            _levelImage.color = levelColor;
            _levelImage.sprite = levelSprite;
            LevelType = levelType;

            ChangeLevelImageSize(aspectRatio);
        }

        private void ChangeLevelImageSize(float aspectRatio)
        {
            RectTransform rectTransform = _levelImage.rectTransform;
            Vector2 size = rectTransform.sizeDelta;
            size.x = size.y * aspectRatio;
            rectTransform.sizeDelta = size;
        }
    }
}