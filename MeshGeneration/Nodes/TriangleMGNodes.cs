namespace UShape.MeshGeneration
{
    public static class TriangleMGNodes
    {
        public static readonly Fill fill = new Fill();
        public static readonly Bridge bridge = new Bridge();
        [System.Serializable]
        public class Fill : IMGNode
        {
            public void Do(MeshGenerationContext contex)
            {
                int start = contex.LatestEdgeStartVertexIndex;
                var polyShape = contex.LatestEdge.polyShape;
                int count = polyShape.Count;
                var mesh = contex.mesh;
                var trises = polyShape.Trises;

                for (int i = 0; i < trises.Count; i++)
                {
                    mesh.trises.Add(trises[i] + start);
                }
            }
        }
        [System.Serializable]
        public class Bridge : IMGNode
        {
            public void Do(MeshGenerationContext contex)
            {
                if (contex.edges.Count < 2) return;
                int count = contex.LatestEdge.polyShape.Count;
                var startIndex = contex.mesh.vertexes.Count - count * 2;

                for (int k = 0; k < count; k++)
                {
                    var cDown = k + startIndex;
                    var cUp = cDown + count;
                    var nDown = ((k + 1) % count) + startIndex;
                    var nUp = nDown + count;
                    contex.mesh.AddTris(cDown, nDown, cUp);
                    contex.mesh.AddTris(nDown, nUp, cUp);
                }

            }
        }
    }
}
