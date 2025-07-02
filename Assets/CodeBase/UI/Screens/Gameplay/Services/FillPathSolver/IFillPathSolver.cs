using UnityEngine;
namespace CodeBase.UI.Screens.Gameplay.Services.FillPathSolver
{
    public interface IFillPathSolver
    {
        float CalculateFillAmount(FillType type, Vector2 startPosition, Vector2 endPosition,
            Vector2 pointerPosition);

        Vector2 GetPositionByFillAmount(FillType type, Vector2 startPosition, Vector2 endPosition, float fillAmount);
    }
}