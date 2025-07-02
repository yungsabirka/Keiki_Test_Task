using System;
using CodeBase.UI.Screens.Gameplay.Services.FillPathSolver;
using UnityEngine;
namespace CodeBase.Data.Levels
{
    [Serializable]
    public class InteractiveImagePathData
    {
        public FillType FillType { get; private set; }
        public Vector2 StartPoint { get; private set; }
        public Vector2 EndPoint { get; private set; }
        public Vector2 InputPoint { get; private set; }

        public InteractiveImagePathData(Vector2 inputPoint, Vector2 endPoint, Vector2 startPoint, FillType fillType)
        {
            InputPoint = inputPoint;
            EndPoint = endPoint;
            StartPoint = startPoint;
            FillType = fillType;
        }
    }
}