using UnityEngine;
using System.Collections.Generic;
namespace CodeBase.UI.Screens.Gameplay.Services.FillPathSolver
{
    public interface IFillPathSolver
    {
        float CalculateFillAmount(FillType type, Vector2 startPosition, Vector2 endPosition,
            Vector2 pointerPosition);

        Vector2 GetPositionByFillAmount(FillType type, Vector2 startPosition, Vector2 endPosition, float fillAmount);

        List<Vector2> GetPath(FillType type, Vector2 start, Vector2 end, float distanceBetweenPoints, float fillAmount);
    }
}