using System.Collections.Generic;
using UnityEngine;
namespace UShape.MeshGeneration
{
    public class DynamicMesh
    {
        public Color32 defaultColor = new Color32(0, 0, 0, 0);
        public List<Vector3> vertexes = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uv = new List<Vector2>();
        public List<Color32> colors = new List<Color32>();
        public int Count
        {
            get
            {
                return vertexes.Count;
            }
        }

        public void Clear()
        {
            vertexes.Clear();
            triangles.Clear();
            uv.Clear();
            colors.Clear();
        }
        public void AddVertex(Vector3 vertexPosition)
        {
            vertexes.Add(vertexPosition);
            colors.Add(defaultColor);
        }
        public void AddTrisIndex(int vertexIndex)
        {
            triangles.Add(vertexIndex);
        }
        public void AddTris(int index1, int index2, int index3)
        {
            AddTrisIndex(index1);
            AddTrisIndex(index2);
            AddTrisIndex(index3);
        }
        public void SetColor(int index, Color color)
        {

            if (index < colors.Count)
            {
                colors[index] = color;
            }
            else
            {
                while (index > (colors.Count))
                {
                    colors.Add(defaultColor);
                }
                colors.Add(color);
            }

        }
        public Color32 GetColor(int index)
        {
            if (index < colors.Count)
            {
                return colors[index];
            }
            return defaultColor;
        }
        public Vector2 GetUV(int index)
        {
            if (index < uv.Count)
            {
                return uv[index];
            }
            return Vector2.zero;
        }
        public void SetUV(int index, Vector2 uv)
        {
            if (index < this.uv.Count)
            {
                this.uv[index] = uv;
            }
            else
            {
                while (index > (this.uv.Count))
                {
                    this.uv.Add(Vector2.zero);
                }
                this.uv.Add(uv);
            }
        }
    }
}
