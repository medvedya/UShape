using UnityEngine;
namespace UShape.PolyGeneration.CircleCorner.ByRect.UIComponents
{
    public class UIEllipseShape : UIPolyGeneratorComponent
    {
        public int segmentCount = 30;
        protected override bool OnGenerate(PolyShape polyShape, Rect rect)
        {
            polyShape.Clear();
            polyShape.DrawEllipse(rect, segmentCount);
            return true;
        }
    }
}
