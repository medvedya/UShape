using System.Collections.Generic;
namespace UShape.MeshGeneration
{
    public class MeshGenerationContext
    {

        public PolyShape polyShape;
        public DynamicMesh mesh = new DynamicMesh();
        public List<EdgeInfo> edges = new List<EdgeInfo>();
        public int stackSize;
        public EdgeInfo LatestEdge
        {
            get
            {
                return edges[edges.Count - 1];
            }
        }
        public bool HasAnyEdge
        {
            get
            {
                return edges.Count > 0;
            }
        }
        public int LatestEdgeStartVertexIndex
        {
            get
            {
                return mesh.vertexes.Count - LatestEdge.polyShape.Count;
            }
        }
        public int Check()
        {
            const int maxStackSize = 256;
            if (stackSize >= maxStackSize) return 1;
            return 0;
        }
        public void Clear()
        {
            mesh.Clear();
            edges.Clear();
            stackSize = 0;
        }

    }
}
