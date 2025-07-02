using CodeBase.Data.Levels;
using CodeBase.UI.Screens.Gameplay.Elements;
using CodeBase.UI.Screens.Gameplay.Elements.Data;
using Cysharp.Threading.Tasks;
namespace CodeBase.UI.Screens.Gameplay.Factory
{
    public interface IGameplayElementsFactory
    {
        UniTask<LevelConstructorView> CreateLevelConstructorView(LevelEnterParams enterParams);

        UniTask Initialize();

        HintView CreateHintView(HintType hintType, float contourSize);
    }
}