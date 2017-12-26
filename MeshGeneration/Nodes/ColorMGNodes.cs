using System;
using UnityEngine;
namespace UShape.MeshGeneration
{
    public static class ColorMGNodes
    {

        [System.Serializable]
        public abstract class BasePaint : MGEdgeModifierNode
        {
            public enum PaintMode : int { AlphaBlending = 1, Replace = 2, None = 0 }
            public PaintMode mode = PaintMode.AlphaBlending;
            public void SetColor(int index, Color color, DynamicMesh mesh)
            {
                switch (mode)
                {
                    case PaintMode.Replace:
                        mesh.SetColor(index, color);
                        break;
                    case PaintMode.AlphaBlending:

                        //https://en.wikipedia.org/wiki/Alpha_compositing
                        Color src = color;
                        Color dst = mesh.GetColor(index);
                        var out_a = src.a + dst.a * (1f - src.a);
                        var out_grba = (src * src.a + dst * dst.a * (1f - src.a)) / out_a;
                        out_grba.a = out_a;
                        mesh.SetColor(index, out_grba);
                        break;
                    default:
                        break;
                }
            }

        }
        [System.Serializable]
        public class ByNormalPaint : BasePaint
        {
            public bool simetric = true;
            public Gradient gradient = new Gradient();
            [Range(0, 1)]
            public float startValue;

            protected override void Do(DynamicMesh mesh, int startMeshIndex, int vertexCount, PolyShape poly)
            {
                float startAngle = startValue * Mathf.PI * 2f;
                var startDir = new Vector2(Mathf.Cos(startAngle), Mathf.Sin(startAngle));
                if (simetric)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var p = poly[i];
                        var t = Vector2.Angle(startDir, p.normal) / 180f;

                        SetColor(startMeshIndex + i, gradient.Evaluate(t), mesh);
                    }
                }
                else
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var p = poly[i];
                        var t = Vector2.SignedAngle(startDir, p.normal) / 360f + 0.5f;
                        SetColor(startMeshIndex + i, gradient.Evaluate(t), mesh);
                    }
                }
            }
        }
        [System.Serializable]
        public class Paint : BasePaint, IMGNode
        {
            public Color color = new Color(1, 1, 1, 1);
            protected override void Do(DynamicMesh mesh, int startMeshIndex, int vertexCount, PolyShape poly)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    SetColor(i + startMeshIndex, color, mesh);
                }
            }
        }
    }
}
