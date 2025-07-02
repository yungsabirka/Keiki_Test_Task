using System;
using System.Collections.Generic;
using UnityEngine;
namespace CodeBase.UI.Screens.Gameplay.Services.FillPathSolver
{
    public class FillPathSolver : IFillPathSolver
    {
        private readonly Dictionary<FillType, IFillAmountStrategy> _strategies = new Dictionary<FillType, IFillAmountStrategy>
        {
            {FillType.Linear, new LinearFillStrategy()},
            {FillType.Radial, new RadialFillStrategy()}
        };

        public float CalculateFillAmount(FillType type, Vector2 startPosition, Vector2 endPosition, Vector2 pointerPosition)
        {
            if (_strategies.TryGetValue(type, out var strategy))
                return strategy.CalculateFillAmount(startPosition, endPosition, pointerPosition);

            throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported fill type: {type}");
        }
        
        public Vector2 GetPositionByFillAmount(FillType type, Vector2 startPosition, Vector2 endPosition, float fillAmount)
        {
            if (_strategies.TryGetValue(type, out var strategy))
                return strategy.GetPositionAlongPath(startPosition, endPosition, fillAmount);

            throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported fill type: {type}");
        }
    }
}