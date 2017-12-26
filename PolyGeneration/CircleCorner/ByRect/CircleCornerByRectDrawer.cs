using System.Collections.Generic;
using UnityEngine;
namespace UShape.PolyGeneration.CircleCorner.ByRect
{
    public static class CircleCornerByRectDrawer
    {
        static List<Corner> cornerList = new List<Corner>();
        static List<Side> sideList = new List<Side>();
        public static void DrawCircleCornerPolyShape(this PolyShape polyShape, Rect rect, IRectSideCornerListProvider sideCornerListPropvider)
        {
            var sideCornerList = sideCornerListPropvider.SideCornerList;
            for (int i = 0; i < sideCornerList.Count; i++)
            {
                var item = sideCornerList[i];
                cornerList.Add(item.corner.ToCorner(rect));
                sideList.Add(item.side.ToSide(rect));
            }
            polyShape.Clear();
            polyShape.DrawCircleCornerPolygon(cornerList, sideList);
            cornerList.Clear();
            sideList.Clear();
        }
    }
}
