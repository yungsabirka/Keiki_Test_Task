using CodeBase.Infrastructure.UI;
using CodeBase.UI.Popups.FingerHint.Data;
using CodeBase.UI.Screens.Gameplay.Elements;
using DG.Tweening;
using R3;
using UnityEngine;
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
            SubscribeForAnimationPathReady();
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

        private void SubscribeForAnimationPathReady()
        {
            var animationPathSubscription = _viewModel
                .AnimationPathReady
                .Subscribe(OnAnimationPathReady);
            _disposable.Add(animationPathSubscription);
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
            var activeFillImage = _levelConstructorView.ActiveFillImage;

            _viewModel.PrepareAnimationPath(
                activeFillImage.GetFillType(),
                activeFillImage.GetStartPosition(),
                activeFillImage.GetEndPosition(),
                activeFillImage.GetContourWidth() / 2,
                activeFillImage.GetFillAmount());
        }

        private void StartFingerAnimation(FingerAnimationPathData pathData)
        {
            _showAndMoveSequence = DOTween.Sequence();
            _showAndMoveSequence.Append(_fingerImage.transform.DOScale(Vector3.one, 0.3f));
            _fingerImage.transform.position = pathData.Positions[0];
            float singleDuration = pathData.PathLength / (pathData.Positions.Count * _fingerSpeed);
            
            for (int i = 1; i < pathData.Positions.Count; i++)
                _showAndMoveSequence.Append(_fingerImage
                    .transform
                    .DOMove(pathData.Positions[i], singleDuration)
                    .SetEase(Ease.Linear));

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

        private void KillFingerTweens()
        {
            _hideTweener?.Kill();
            _showAndMoveSequence?.Kill();
        }

        private void OnAnimationPathReady(FingerAnimationPathData pathData) => 
            StartFingerAnimation(pathData);
    }
}