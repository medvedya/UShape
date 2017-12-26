using System;
using UnityEngine;
namespace UShape.MeshGeneration
{
    public static class UVMGNodes
    {
        [System.Serializable]
        public class Tille : MGEdgeModifierNode
        {
            public bool useMeshVertex = true;
            public float aspectRatio = 1f;
            public float texturesPerUnit;
            protected override void Do(DynamicMesh mesh, int startMeshIndex, int vertexCount, PolyShape poly)
            {
                var s = new Vector2(texturesPerUnit * aspectRatio, texturesPerUnit);

                for (int i = 0; i < vertexCount; i++)
                {
                    int meshIndex = startMeshIndex + i;
                    mesh.SetUV(meshIndex, Vector2.Scale(s, useMeshVertex ? (Vector2)mesh.vertexes[meshIndex] : poly.positions[i]));
                }
            }
        }
        [System.Serializable]
        public class ByPath : IMGNode
        {
            public float texturesPerUnit;
            [Range(0, 1)]
            public float[] UVPositions;
            public void Do(MeshGenerationContext contex)
            {
                if (UVPositions == null || UVPositions.Length == 0 || !contex.HasAnyEdge ) return;
                var a = contex.LatestEdge.polyShape.Path;
                float textureSegmentCount = Mathf.Round(contex.LatestEdge.polyShape.Length * texturesPerUnit);
                float lastEdgeLength = contex.LatestEdge.polyShape.Length;

                var startIndex = contex.mesh.vertexes.Count;
                for (int i = 0; i < UVPositions.Length; i++)
                {
                    var uvPos = UVPositions[UVPositions.Length - 1 - i];
                    var polyShape = contex.edges[contex.edges.Count - i - 1].polyShape;
                    startIndex -= polyShape.Count;
                    var factor = polyShape.Length / lastEdgeLength;
                    var path = polyShape.Path;
                    for (int j = 0; j < polyShape.Count; j++)
                    {
                        contex.mesh.SetUV(j + startIndex, new Vector2(path[j] / lastEdgeLength * textureSegmentCount * factor, uvPos));
                        // contex.mesh.SetColor(j + startIndex, Color.Lerp(Color.blue, Color.red, path[j] / lastEdgeLength));

                    }
                }
            }
        }
    }
}
