using CodeBase.UI.Screens.Gameplay.Elements.Data;
using R3;
namespace CodeBase.UI.Screens.Gameplay.Elements
{
    public interface IHintsContainer
    {
        public Subject<HintsPathData> HintsRequested { get; }
        public Subject<Unit> HintsHideRequested { get; }
        public Subject<Unit> Disabled { get; }
        void AddHintView(HintView hint);
    }
}