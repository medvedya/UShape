using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace UShape.MeshGeneration.Nodes
{
    public class MGNodeSetComponent : MonoBehaviour, IMGNode
    {
        public MGNodeSet nodeSet = new MGNodeSet();
        public void Do(MeshGenerationContext context)
        {
            nodeSet.Do(context);
        }
    }
}
