using UnityEngine;
namespace CodeBase.UI.Screens.Gameplay.Elements.Data
{
    public class LevelConstructorViewData
    {
        public Color LevelColor { get; private set; }
        public LevelConstructorViewData(Color levelColor) =>
            LevelColor = levelColor;
    }
}