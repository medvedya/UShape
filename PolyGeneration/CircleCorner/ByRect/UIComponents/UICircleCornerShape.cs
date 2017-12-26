using System;
using System.Collections.Generic;
using UnityEngine;
using UShape.Libs;

namespace UShape.PolyGeneration.CircleCorner.ByRect.UIComponents
{
    [ExecuteInEditMode]
    public class UICircleCornerShape : UIPolyGeneratorComponent, IRectSideCornerListProvider
    {
        [SerializeField]
        UniversalRectSideCornerListProvider sideCornerListProvider = new UniversalRectSideCornerListProvider();
        public List<RectSideCornerPara> SideCornerList
        {
            get
            {
                return sideCornerListProvider.SideCornerList;
            }
        }
       

        protected override bool OnGenerate(PolyShape polyShape, Rect rect)
        {
            polyShape.Clear();
            polyShape.DrawCircleCornerPolyShape(rect, this);
            return true;
        }
        [ContextMenu("Four corner shape")]
        public void ResetTo4CornersShape()
        {
            sideCornerListProvider.ResetTo4CornersHorizontal(60, 20);
        }
        [ContextMenu("Two corner shape")]
        public void ResetTo2CornerHorizontalShape()
        {
            sideCornerListProvider.ResetTo2CornersHorizontal(60);
        }
    }

}
