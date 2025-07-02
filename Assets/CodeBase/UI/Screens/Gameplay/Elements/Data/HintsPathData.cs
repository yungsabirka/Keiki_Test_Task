using CodeBase.UI.Screens.Gameplay.Services.FillPathSolver;
using UnityEngine;
namespace CodeBase.UI.Screens.Gameplay.Elements.Data
{
    public class HintsPathData
    {
        public Vector2 StartPosition { get; private set; }
        public Vector2 EntPosition { get; private set; }
        public FillType FillType { get; private set; }
        public float ContourWidth { get; private set; }

        public HintsPathData(Vector2 startPosition, Vector2 entPosition, FillType fillType, float contourWidth)
        {
            StartPosition = startPosition;
            EntPosition = entPosition;
            FillType = fillType;
            ContourWidth = contourWidth;
        }
    }
}