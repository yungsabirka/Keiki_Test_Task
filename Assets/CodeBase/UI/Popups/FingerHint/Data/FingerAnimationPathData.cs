using System.Collections.Generic;
using UnityEngine;
namespace CodeBase.UI.Popups.FingerHint.Data
{
    public class FingerAnimationPathData
    {
        public List<Vector2> Positions { get; }
        public float PathLength { get; }

        public FingerAnimationPathData(List<Vector2> positions, float pathLength)
        {
            Positions = positions;
            PathLength = pathLength;
        }
    }
}