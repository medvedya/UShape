using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace UShape.PolyGeneration
{
    [ExecuteInEditMode]
    public class Debuger : MonoBehaviour
    {
        public float normalSize = 1;
        public InterfaceObject_IPolyShapeProvider polyShapeProvider = new InterfaceObject_IPolyShapeProvider();
        private void Update()
        {
            if (polyShapeProvider.UsedObject != null)
            {
                var polyShape = polyShapeProvider.UsedObject.Poly;
                if (polyShape != null)
                {
                    for (int i = 0; i < polyShape.Count; i++)
                    {
                        var p1 = polyShape[i];
                        var p2 = polyShape[(i + 1) % polyShape.Count];
                        Debug.DrawLine(transform.TransformPoint(p1.position), transform.TransformPoint(p2.position));
                        Debug.DrawRay(transform.TransformPoint(p1.position), p1.normal * normalSize);
                    }
                }
            }
        }
    }
}
