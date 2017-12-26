using System;
using UShape.Libs;
namespace UShape
{
    public interface IPolyShapeProvider
    {
        PolyShape Poly { get; }
        event Action<IPolyShapeProvider> OnPolyShapeChange;
    }
    [System.Serializable]
    public class InterfaceObject_IPolyShapeProvider : InterfaceObject<IPolyShapeProvider>
    { }
}
