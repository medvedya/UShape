using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UShape
{
    public struct Point
    {
        public Vector2 position;
        public Vector2 normal;
    }

    public class PolyShape
    {
        Triangulator triangulator = new Triangulator();

        private bool dirtyTris = true;
        private bool dirtyPath = true;
        public List<Vector2> normals = new List<Vector2>();
        public List<Vector2> positions = new List<Vector2>();

        public List<int> Trises
        {
            get
            {
                if (dirtyTris)
                {
                    trises.Clear();
                    triangulator.Triangulate(positions, trises);
                    dirtyTris = false;
                }
                return trises;
            }
        }
        private List<int> trises = new List<int>();

        public float Length { get; private set; }

        public List<float> Path
        {
            get
            {
                if (dirtyPath)
                {
                    CalcPath();
                }
                return path;

            }
        }
        private List<float> path = new List<float>();
        public Point this[int i]
        {
            get
            {
                return new Point() { position = positions[i], normal = normals[i] };
            }
        }
        public int Count
        {
            get
            {
                return positions.Count;
            }
        }
        public void Clear()
        {
            normals.Clear();
            positions.Clear();
            dirtyTris = true;
        }
        public void Add(Point point)
        {
            Add(point.position, point.normal);
        }
        public void Add(Vector2 point, Vector2 normal)
        {
            positions.Add(point);
            normals.Add(normal);
            dirtyTris = true;
            dirtyPath = true;
        }

        public bool TryAddPoint(Point point)
        {
            return TryAddPoint(point.position, point.normal);
        }
        public bool TryAddPoint(Vector2 point, Vector2 normal)
        {
            dirtyTris = true;
            dirtyPath = true;
            if (positions.Count == 0)
            {
                Add(point, normal);
                return true;
            }
            var lastPoint = positions[positions.Count - 1];
            if (Vector2.SqrMagnitude(lastPoint - point) < Mathf.Epsilon)
            {
                normals[positions.Count - 1] = normal;
                return false;
            }
            Add(point, normal);
            return true;
        }
        private void CalcPath()
        {
            path.Clear();
            var p = 0f;
            path.Add(0);
            for (int i = 1; i < positions.Count; i++)
            {
                var p1 = positions[i - 1];
                var p2 = positions[i];
                p += Vector2.Distance(p1, p2);
                path.Add(p);
            }
            Length = Vector2.Distance(positions[0], positions[Count - 1]) + p;
            dirtyPath = false;
        }
    }
}
