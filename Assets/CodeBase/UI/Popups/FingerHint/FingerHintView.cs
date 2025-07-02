using CodeBase.Infrastructure.UI;
using CodeBase.UI.Screens.Gameplay.Elements;
using CodeBase.UI.Screens.Gameplay.Services.FillPathSolver;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace CodeBase.UI.Popups.FingerHint
{
    public class FingerHintView : PopupViewBase
    {
        [SerializeField] private Image _fingerImage;
        [SerializeField] private float _fingerSpeed;

        private readonly CompositeDisposable _disposable = new();

        private FingerHintViewModel _viewModel;
        private LevelConstructorView _levelConstructorView;
        private Tweener _hideTweener;
        private Sequence _showAndMoveSequence;

        private void OnDestroy()
        {
            KillFingerTweens();
            _disposable?.Dispose();
            _viewModel?.Dispose();
        }

        public void Initialize(FingerHintViewModel viewModel, LevelConstructorView levelConstructorView)
        {
            _levelConstructorView = levelConstructorView;
            _viewModel = viewModel;
            SubscribeForVisualHintRequested();
            SubscribeForVisualHintHideRequested();
            _fingerImage.transform.localScale = Vector3.zero;
        }

        private void SubscribeForVisualHintHideRequested()
        {
            var visualHintHideSubscription = _viewModel
                .VisualHintHideRequested
                .Subscribe(_ => OnVisualHintHideRequested());
            _disposable.Add(visualHintHideSubscription);
        }

        private void SubscribeForVisualHintRequested()
        {
            var visualHintRequestedSubscription = _viewModel
                .VisualHintRequested
                .Subscribe(_ => OnVisualHintRequested());
            _disposable.Add(visualHintRequestedSubscription);
        }

        private void OnVisualHintHideRequested()
        {
            KillFingerTweens();
            _fingerImage
                .transform
                .DOScale(Vector3.zero, 0.3f);
        }

        private void OnVisualHintRequested()
        {
            KillFingerTweens();
            UpdateFingerSize();
            _showAndMoveSequence = DOTween.Sequence();
            _showAndMoveSequence.Append(_fingerImage.transform.DOScale(Vector3.one, 0.3f));

            var activeFillImage = _levelConstructorView.ActiveFillImage;
            var fillType = activeFillImage.GetFillType();
            Vector2 start = GetFingerStartPosition();
            Vector2 end = activeFillImage.GetEndPosition();

            _fingerImage.transform.position = start;

            switch (fillType)
            {
                case FillType.Linear:
                    AddLinearMoving(end, start);
                    break;
                case FillType.Radial:
                    AddRadialMoving(activeFillImage, start);
                    break;
            }
            _showAndMoveSequence.Append(_fingerImage.transform.DOScale(Vector3.zero, 0.3f));
            _showAndMoveSequence.AppendInterval(3f);
            _showAndMoveSequence.SetLoops(-1, LoopType.Restart);
            _showAndMoveSequence.Play();
        }

        private void UpdateFingerSize()
        {
            float contourWidth = _levelConstructorView.ActiveFillImage.GetContourWidth();
            float fingerAspectRatio = _fingerImage.rectTransform.rect.width / _fingerImage.rectTransform.rect.height;
            _fingerImage.rectTransform.sizeDelta = new Vector2(contourWidth, fingerAspectRatio * contourWidth);
        }

        private void AddLinearMoving(Vector2 end, Vector2 start)
        {
            float duration = Vector2.Distance(end, start) / _fingerSpeed;
            _showAndMoveSequence.Append(_fingerImage.transform.DOMove(end, duration));
        }

        private void AddRadialMoving(InteractiveFillImage activeFillImage, Vector2 start)
        {
            Vector2 startPosition = activeFillImage.GetStartPosition();
            Vector2 endPosition = activeFillImage.GetEndPosition();
            Vector2 center = (startPosition + endPosition) * 0.5f;
            float radius = Vector2.Distance(center, startPosition);

            Vector2 fromCenterToStart = start - center;
            Vector2 fromCenterToTarget = startPosition - center;

            float startAngle = Mathf.Atan2(fromCenterToStart.y, fromCenterToStart.x) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(fromCenterToTarget.y, fromCenterToTarget.x) * Mathf.Rad2Deg;

            startAngle = NormalizeAngle(startAngle);
            targetAngle = NormalizeAngle(targetAngle);
            float angleDelta = Vector2.Distance(start, startPosition) < 0.01f
                ? 360f
                : (startAngle - targetAngle + 360f) % 360f;
            ;

            int segments = Mathf.Max(1, Mathf.CeilToInt(angleDelta / 5f));
            float duration = angleDelta / 180f;

            for (int i = 1; i <= segments; i++)
            {
                float t = i / (float)segments;
                float angle = startAngle - angleDelta * t;
                float rad = angle * Mathf.Deg2Rad;
                Vector2 point = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

                _showAndMoveSequence.Append(_fingerImage
                    .transform
                    .DOMove(point, duration / segments)
                    .SetEase(Ease.Linear));
            }
        }


        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }

        private Vector2 GetFingerStartPosition()
        {
            InteractiveFillImage activeFillImage = _levelConstructorView.ActiveFillImage;
            return _viewModel.GetPositionByFillAmount(activeFillImage.GetFillType(), activeFillImage.GetStartPosition(),
                activeFillImage.GetEndPosition(), activeFillImage.GetFillAmount());
        }

        private void KillFingerTweens()
        {
            _hideTweener?.Kill();
            _showAndMoveSequence?.Kill();
        }
    }
}