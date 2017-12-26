using System;
using UnityEngine;
namespace UShape.MeshGeneration
{
    public static class VertexMGNodes
    {
        public static readonly AddEdge addEdge = new AddEdge();
        [System.Serializable]
        public class AddEdge : IMGNode
        {
            public void Do(MeshGenerationContext contex)
            {
                for (int i = 0; i < contex.polyShape.Count; i++)
                {
                    contex.mesh.AddVertex(contex.polyShape.positions[i]);
                }
                contex.edges.Add(new EdgeInfo() { polyShape = contex.polyShape });
            }
        }
        [System.Serializable]
        public class Extrude : MGEdgeModifierNode
        {
            public float value;
            protected override void Do(DynamicMesh mesh, int startMeshIndex, int vertexCount, PolyShape poly)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    mesh.vertexes[startMeshIndex + i] += ((Vector3)(poly.normals[i] * value));
                }
            }
        }
        [System.Serializable]
        public class Offset : MGEdgeModifierNode
        {
            public Vector3 value;
            protected override void Do(DynamicMesh mesh, int startMeshIndex, int vertexCount, PolyShape poly)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    mesh.vertexes[startMeshIndex + i] += value;
                }
            }
        }
        public static readonly Clone clone = new Clone();
        [System.Serializable]
        public class Clone : IMGNode
        {
            public void Do(MeshGenerationContext context)
            {
                if (!context.HasAnyEdge) return;
                int start = context.LatestEdgeStartVertexIndex;
                var polyShape = context.LatestEdge.polyShape;
                int count = polyShape.Count;
                var mesh = context.mesh;
                for (int i = 0; i < count; i++)
                {
                    mesh.AddVertex(mesh.vertexes[start + i]);
                }
                context.edges.Add(context.LatestEdge);
            }
        }
        [System.Serializable]
        public class ByNormalExtrude : MGEdgeModifierNode
        {
            public AnimationCurve curve = new AnimationCurve();
            public float factor = 1;
            public bool simetric = true;
            [Range(0, 1)]
            public float startValue;
            protected override void Do(DynamicMesh mesh, int startMeshIndex, int vertexCount, PolyShape poly)
            {
                float startAngle = startValue * Mathf.PI * 2f;
                var dir = new Vector2(Mathf.Cos(startAngle), Mathf.Sin(startAngle));
                if (simetric)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var p = poly[i];
                        var t = Vector2.Angle(dir, p.normal) / 180f;
                        var value = curve.Evaluate(t) * factor;
                        mesh.vertexes[startMeshIndex + i] += ((Vector3)(poly.normals[i] * value));
                    }
                }
                else
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var p = poly[i];
                        var t = Vector2.SignedAngle(dir, p.normal) / 360f + 0.5f;
                        var value = curve.Evaluate(t) * factor;
                        mesh.vertexes[startMeshIndex + i] += ((Vector3)(poly.normals[i] * value));
                    }

                }
            }
        }
    }
}
