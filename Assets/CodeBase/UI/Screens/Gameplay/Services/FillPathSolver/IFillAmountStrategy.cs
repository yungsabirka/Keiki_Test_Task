using UnityEngine;
using System.Collections.Generic;
namespace CodeBase.UI.Screens.Gameplay.Services.FillPathSolver
{
    public interface IFillAmountStrategy
    {
        float CalculateFillAmount(Vector2 startPosition, Vector2 endPosition, Vector2 pointerPosition);
        
        Vector2 GetPositionAlongPath(Vector2 startPosition, Vector2 endPosition, float fillAmount);

        List<Vector2> GetPath(Vector2 start, Vector2 end, float distanceBetweenPoints, float fillAmount);
    }
}