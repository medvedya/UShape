namespace UShape.MeshGeneration
{
    public static partial class ComplexMGNodes
    {
        [System.Serializable]
        public class UniversalEdge : IMGNode
        {
            public enum ComplexAddEdgeMode { Fill = 0, Bridge = 1, Line = 2 }
            public ComplexAddEdgeMode mode;
            public bool relative = true;
            public bool usePaint = false;
            [MGEdgeModifierNodeWithoutEdgeCount]
            public VertexMGNodes.Extrude extrude1;
            [MGEdgeModifierNodeWithoutEdgeCount]
            public VertexMGNodes.Extrude extrude2;
            [MGEdgeModifierNodeWithoutEdgeCount]
            public ColorMGNodes.Paint paint1;
            [MGEdgeModifierNodeWithoutEdgeCount]
            public ColorMGNodes.Paint paint2;

            public void Do(MeshGenerationContext contex)
            {
                if (contex.HasAnyEdge && relative)
                {
                    VertexMGNodes.clone.Do(contex);
                }
                else
                {
                    VertexMGNodes.addEdge.Do(contex);
                }
                extrude1.Do(contex);
                if (usePaint)
                {
                    paint1.Do(contex);
                }
                switch (mode)
                {
                    case ComplexAddEdgeMode.Fill:
                        TriangleMGNodes.fill.Do(contex);
                        break;
                    case ComplexAddEdgeMode.Bridge:
                        TriangleMGNodes.bridge.Do(contex);
                        break;
                    case ComplexAddEdgeMode.Line:
                        VertexMGNodes.clone.Do(contex);
                        extrude2.Do(contex);
                        if (usePaint)
                        {
                            paint2.Do(contex);
                        }
                        TriangleMGNodes.bridge.Do(contex);
                        break;
                    default:
                        break;
                }
            }
        }

        [System.Serializable]
        public class Link : IMGNode
        {

            public InterfaceComponent_IMGNode node;

            public void Do(MeshGenerationContext contex)
            {
                if (contex.Check() != 0) return;

                if (node.UsedObject != null)
                {
                    node.UsedObject.Do(contex);
                }
                contex.stackSize++;
            }
        }
    }
}
