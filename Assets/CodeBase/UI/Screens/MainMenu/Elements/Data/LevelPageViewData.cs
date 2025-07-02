using CodeBase.Data.Levels;
using UnityEngine;
namespace CodeBase.UI.Screens.MainMenu.Elements.Data
{
    public class LevelPageViewData
    {
        public LevelType LevelType { get; private set; }
        public Sprite Sprite { get; private set; }
        public Color Color { get; private set; }
        public float AspectRatio { get; private set; }

        public LevelPageViewData(LevelType levelType, Sprite sprite, Color color, float aspectRatio)
        {
            LevelType = levelType;
            Sprite = sprite;
            Color = color;
            AspectRatio = aspectRatio;
        }
    }
}