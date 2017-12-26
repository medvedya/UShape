using UnityEngine;
namespace UShape.PolyGeneration.CircleCorner
{
    [System.Serializable]
    public enum SideMode { Flat = 0, CubicBezierCurve = 1 }
    public struct Side
    {
        public Vector2 p1;
        public Vector2 p2;
        public int segmentCount;
        public SideMode mode;
    }
}

