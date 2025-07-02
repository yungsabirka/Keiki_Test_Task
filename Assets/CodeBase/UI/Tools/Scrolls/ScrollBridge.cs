using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace CodeBase.UI.Tools.Scrolls
{
    public class ScrollBridge : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private ScrollRect _scrollRect;

        private ScrollRect _parentScroll;
        private bool _isVerticalDrag;

        public void SetParentScroll(ScrollRect scrollRect) => _parentScroll = scrollRect;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_parentScroll == null)
                return;

            _isVerticalDrag = Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x);

            if (!_isVerticalDrag)
                return;
            _parentScroll.OnBeginDrag(eventData);
            _scrollRect.enabled = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_parentScroll == null)
                return;

            if (_isVerticalDrag)
                _parentScroll.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_parentScroll == null)
                return;

            if (_isVerticalDrag)
                _parentScroll.OnEndDrag(eventData);

            _scrollRect.enabled = true;
        }
    }
}