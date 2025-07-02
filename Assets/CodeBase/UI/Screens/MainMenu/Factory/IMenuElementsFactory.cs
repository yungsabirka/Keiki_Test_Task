using CodeBase.Data.Levels;
using CodeBase.UI.Screens.MainMenu.Elements;
using Cysharp.Threading.Tasks;
namespace CodeBase.UI.Screens.MainMenu.Factory
{
    public interface IMenuElementsFactory
    {
        UniTask Initialize();

        HorizontalLevelsContainer CreateLevelContainer(LevelData levelData);

        LevelPageView CreateUnsetLevelPageView();
    }
}