using UnityEngine;
using System.Collections.Generic;
namespace CodeBase.UI.Screens.Gameplay.Services.FillPathSolver
{
    public class RadialFillStrategy : IFillAmountStrategy
    {
        public float CalculateFillAmount(Vector2 startPosition, Vector2 endPosition, Vector2 pointerPosition)
        {
            Vector2 center = (startPosition + endPosition) * 0.5f;

            Vector2 fromCenterToStart = startPosition - center;
            Vector2 fromCenterToEnd = endPosition - center;
            Vector2 fromCenterToPointer = pointerPosition - center;

            float startAngle = Mathf.Atan2(fromCenterToStart.y, fromCenterToStart.x) * Mathf.Rad2Deg;
            float pointerAngle = Mathf.Atan2(fromCenterToPointer.y, fromCenterToPointer.x) * Mathf.Rad2Deg;

            startAngle = NormalizeAngle(startAngle);
            pointerAngle = NormalizeAngle(pointerAngle);

            float deltaAngle = (pointerAngle - startAngle + 360f) % 360f;

            float cross = fromCenterToStart.x * fromCenterToEnd.y - fromCenterToStart.y * fromCenterToEnd.x;
            bool isClockwise = cross < 0;

            if (!isClockwise)
                deltaAngle = 360f - deltaAngle;

            float fillAmount = Mathf.Clamp01(deltaAngle / 360f);
            return fillAmount;
        }
        
        public Vector2 GetPositionAlongPath(Vector2 startPosition, Vector2 endPosition, float fillAmount)
        {
            Vector2 center = (startPosition + endPosition) * 0.5f;
            Vector2 fromCenterToStart = startPosition - center;
            Vector2 fromCenterToEnd = endPosition - center;

            float radius = fromCenterToStart.magnitude;
            float startAngle = Mathf.Atan2(fromCenterToStart.y, fromCenterToStart.x);

            float cross = fromCenterToStart.x * fromCenterToEnd.y - fromCenterToStart.y * fromCenterToEnd.x;
            bool isClockwise = cross < 0;

            float angleOffset = (isClockwise ? 1f : -1f) * fillAmount * Mathf.PI * 2f;
            float angle = startAngle + angleOffset;

            Vector2 positionOnCircle = new Vector2(
                center.x + radius * Mathf.Cos(angle),
                center.y + radius * Mathf.Sin(angle)
            );

            return positionOnCircle;
        }

        public List<Vector2> GetPath(Vector2 start, Vector2 end, float distanceBetweenPoints, float fillAmount)
        {
            float fullCircleRad = 2f * Mathf.PI;

            Vector2 center = (start + end) * 0.5f;
            float radius = Vector2.Distance(start, end) * 0.5f;
            Vector2 radiusVector = start - center;

            float totalArcLength = radius * fullCircleRad;
            int count = Mathf.Max(1, Mathf.FloorToInt(totalArcLength / distanceBetweenPoints));

            float startAngle = Mathf.Atan2(radiusVector.y, radiusVector.x);
            float filledAngle = fillAmount * fullCircleRad;

            var path = new List<Vector2>(count);

            for (int i = 0; i < count; i++)
            {
                float t = i / (float)(count - 1);
                float angle = -(startAngle + filledAngle + t * (fullCircleRad - filledAngle));

                Vector2 point = new Vector2(
                    center.x + radius * Mathf.Cos(angle),
                    center.y + radius * Mathf.Sin(angle)
                );

                path.Add(point);
            }

            return path;
        }


        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }
    }

}