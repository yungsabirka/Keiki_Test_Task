using System.Collections.Generic;
using CodeBase.Data.Levels;
using CodeBase.UI.Screens.Gameplay.Elements.Data;
using DG.Tweening;
using R3;
using UnityEngine;
namespace CodeBase.UI.Screens.Gameplay.Elements
{
    public class LevelConstructorView : MonoBehaviour, IHintsContainer
    {
        [SerializeField] private Transform _hintsContainer;
        [SerializeField] private SpaceShipView _spaceShipView;

        private readonly CompositeDisposable _disposables = new();
        private Tweener _spaceShipTweener;

        [field: SerializeField] public List<InteractiveFillImage> InteractiveFillImages { get; private set; }

        public InteractiveFillImage ActiveFillImage { get; private set; }
        public Subject<InteractiveImagePathData> SpaceShipDragging { get; } = new();
        public Subject<Unit> SpaceShipDragStarted { get; } = new();
        public Subject<Unit> SpaceShipDragEnded { get; } = new();
        public Subject<HintsPathData> HintsRequested { get; } = new();
        public Subject<Unit> HintsHideRequested { get; } = new();
        public Subject<Unit> Disabled { get; } = new();

        private void OnEnable()
        {
            _disposables.Add(_spaceShipView.Dragging.Subscribe(OnSpaceShipDragging));
            _disposables.Add(_spaceShipView.DragStarted.Subscribe(_ => OnSpaceShipDragStarted()));
            _disposables.Add(_spaceShipView.DragEnded.Subscribe(_ => OnSpaceShipDragEnded()));
        }

        private void OnDisable()
        {
            _disposables?.Dispose();
            _spaceShipTweener?.Kill();
            Disabled.OnNext(Unit.Default);
        }

        public void Initialize(Color filledImageColor)
        {
            foreach (InteractiveFillImage interactiveFillImage in InteractiveFillImages)
            {
                interactiveFillImage.SetColor(filledImageColor);
                interactiveFillImage.FillImage(0f);
            }
        }

        public void MoveSpaceShipToPosition(Vector2 position)
        {
            _spaceShipView.DisableDrag();
            float distance = Vector2.Distance(position, _spaceShipView.transform.position);
            float duration = distance / _spaceShipView.AnimationMovingSpeed;
            _spaceShipTweener = _spaceShipView
                .transform
                .DOMove(position, duration)
                .OnComplete(_spaceShipView.EnableDrag);
        }

        public void SetSpaceShipPosition(Vector2 position) =>
            _spaceShipView.transform.position = position;

        public void SetActiveFillImage(InteractiveFillImage fillImage) =>
            ActiveFillImage = fillImage;

        public void AddHintView(HintView hint) =>
            hint.transform.SetParent(_hintsContainer);

        public void ShowHints() =>
            HintsRequested.OnNext(new HintsPathData(
                ActiveFillImage.GetStartPosition(),
                ActiveFillImage.GetEndPosition(),
                ActiveFillImage.GetFillType(),
                ActiveFillImage.GetContourWidth()));
        
        public void HideHints() =>
            HintsHideRequested.OnNext(Unit.Default);

        private void OnSpaceShipDragging(Vector2 inputPosition)
        {
            InteractiveImagePathData pathData = new(inputPosition,
                ActiveFillImage.GetEndPosition(), ActiveFillImage.GetStartPosition(), ActiveFillImage.GetFillType());
            SpaceShipDragging.OnNext(pathData);
        }

        private void OnSpaceShipDragEnded() =>
            SpaceShipDragEnded.OnNext(Unit.Default);

        private void OnSpaceShipDragStarted() =>
            SpaceShipDragStarted.OnNext(Unit.Default);
    }
}