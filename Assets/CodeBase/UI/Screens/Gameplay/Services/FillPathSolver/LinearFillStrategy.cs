using UnityEngine;
using System.Collections.Generic;
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

        public List<Vector2> GetPath(Vector2 start, Vector2 end, float distanceBetweenPoints, float fillAmount)
        {
            if (fillAmount > 0f)
                start = GetPositionAlongPath(start, end, fillAmount);

            float pathLength = Vector2.Distance(start, end);
            int count = Mathf.Max(1, Mathf.FloorToInt(pathLength / distanceBetweenPoints));

            var path = new List<Vector2>(count);

            for (int i = 0; i < count; i++)
            {
                float t = (i + 1f) / (count + 1f);
                path.Add(Vector2.Lerp(start, end, t));
            }
            return path;
        }


        public Vector2 GetPositionAlongPath(Vector2 startPosition, Vector2 endPosition, float fillAmount) =>
            Vector2.Lerp(startPosition, endPosition, Mathf.Clamp01(fillAmount));
    }
}