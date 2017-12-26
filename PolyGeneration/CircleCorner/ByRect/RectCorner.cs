using UnityEngine;
namespace UShape.PolyGeneration.CircleCorner.ByRect
{
    [System.Serializable]
    public struct RectCorner
    {
        public enum Orenatiotion { Horizontal, Vertical }
        public AnchorPoint position;
        public float diameterRatio;
        public float diameterOffset;
        public int segmentCount;
        public Orenatiotion orenatiotion;

        public float CalcRadius(Rect rect)
        {
            return (orenatiotion == Orenatiotion.Vertical ? rect.width : rect.height) * diameterRatio / 2f + diameterOffset / 2f;
        }
        public Corner ToCorner(Rect rect)
        {
            return new Corner()
            {
                position = position.CalcPos(rect),
                radius = CalcRadius(rect),
                segmentCount = segmentCount,
            };
        }
    }
}
