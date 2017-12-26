using UShape.Libs;
namespace UShape.MeshGeneration
{
    [System.Serializable]
    public class InterfaceComponent_IMGNode : InterfaceObject<IMGNode>
    {
    }
    public interface IMGNode
    {
        void Do(MeshGenerationContext context);
    }
}
