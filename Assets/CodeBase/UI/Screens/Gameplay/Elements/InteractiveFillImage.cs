using CodeBase.UI.Screens.Gameplay.Services.FillPathSolver;
using UnityEngine;
using UnityEngine.UI;
namespace CodeBase.UI.Screens.Gameplay.Elements
{
    public class InteractiveFillImage : MonoBehaviour
    {
        [SerializeField] private RectTransform _startPoint;
        [SerializeField] private RectTransform _endPoint;
        [SerializeField] private FillType _fillType;
        [SerializeField] private Image _fillImage;
        [SerializeField] private RectTransform _contourRect;

        public void FillImage(float fillAmount) =>
            _fillImage.fillAmount = fillAmount;

        public void SetColor(Color color) =>
            _fillImage.color = color;

        public void SetActive(bool active) =>
            _fillImage.gameObject.SetActive(active);
        
        public Vector2 GetStartPosition() =>
            _startPoint.transform.position;
        
        public Vector2 GetEndPosition() =>
            _endPoint.transform.position;

        public FillType GetFillType() =>
            _fillType;
        
        public float GetContourWidth() =>
            _contourRect.rect.width;
        
        public float GetFillAmount() =>
            _fillImage.fillAmount;
    }
}