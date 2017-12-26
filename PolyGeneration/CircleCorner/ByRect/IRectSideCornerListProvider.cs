using System.Collections.Generic;
using UShape.Libs;

namespace UShape.PolyGeneration.CircleCorner.ByRect
{
    public interface IRectSideCornerListProvider
    {
        List<RectSideCornerPara> SideCornerList { get; }
    }
    [System.Serializable]
    public class InterfaceComponent_IRectSideCornerListProvider : InterfaceObject<IRectSideCornerListProvider>
    { }
}
