using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace CodeBase.UI.Screens.Gameplay.Elements
{
    [RequireComponent(typeof(Image))]
    public class SpaceShipView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _spaceShipImage;
        
        [field: SerializeField] public float AnimationMovingSpeed { get; private set; }

        public Subject<Vector2> Dragging { get; } = new();
        public Subject<Unit> DragStarted { get; } = new();
        public Subject<Unit> DragEnded { get; } = new();

        public void OnDrag(PointerEventData eventData)
        {
            if(InsideImage(eventData) == false)
                return;
            Dragging?.OnNext(eventData.position);
        }

        public void OnBeginDrag(PointerEventData eventData) =>
            DragStarted?.OnNext(Unit.Default);

        public void OnEndDrag(PointerEventData eventData) =>
            DragEnded?.OnNext(Unit.Default);

        public void EnableDrag() => 
            _spaceShipImage.raycastTarget = true;

        public void DisableDrag() =>
            _spaceShipImage.raycastTarget = false;

        private bool InsideImage(PointerEventData eventData) =>
            RectTransformUtility.RectangleContainsScreenPoint(_spaceShipImage.rectTransform, eventData.position);
    }
}