using UnityEngine;
namespace CodeBase.UI.Screens.Gameplay.Services.FillPathSolver
{
    public class LinearFillStrategy : IFillAmountStrategy
    {
        public float CalculateFillAmount(Vector2 startPosition, Vector2 endPosition, Vector2 pointerPosition)
        {
            Vector2 pathDirection = endPosition - startPosition;
            float pathLength = pathDirection.magnitude;
            Vector2 directionNormalized = pathDirection.normalized;
            Vector2 toPointer = pointerPosition - startPosition;
            float projectedLength = Vector2.Dot(directionNormalized, toPointer);

            float progress = Mathf.Clamp01(projectedLength / pathLength);
            return progress;
        }
        
        public Vector2 GetPositionAlongPath(Vector2 startPosition, Vector2 endPosition, float fillAmount) =>
            Vector2.Lerp(startPosition, endPosition, Mathf.Clamp01(fillAmount));
    }
}