using System;
using System.Collections.Generic;
using UnityEngine;
using UShape.Libs;

namespace UShape.MeshGeneration
{
    [System.Serializable]
    public class MGNodeSet : IMGNode
    {
        public abstract class NodeCollection
        {
            public abstract int Add();
            public abstract void Remove(int index);
            public abstract IMGNode this[int index] { get; }
            public abstract int TypeCode
            {
                get;
            }
            public abstract Type NodeType
            {
                get;
            }
            public abstract int Count { get; }
            public abstract void CopyTo(NodeCollection target);
        }
        public abstract class NodeCollection<T> : NodeCollection where T : IMGNode, new()
        {
            [UnityEngine.SerializeField]
            [NoPrefixLabel]
            List<T> nodes = new List<T>();

            public override IMGNode this[int index]
            {
                get
                {
                    return nodes[index];
                }
            }

            public override int Add()
            {
                nodes.Add(new T());
                return nodes.Count - 1;
            }

            public override void Remove(int index)
            {
                nodes.RemoveAt(index);
            }
            public override Type NodeType
            {
                get
                {
                    return typeof(T);
                }
            }
            public override int Count
            {
                get
                {
                    return nodes.Count;
                }
            }
            public override void CopyTo(NodeCollection target)
            {
                var _target = target as NodeCollection<T>;
                _target.nodes.Clear();
                _target.nodes.AddRange(nodes);
            }

        }

        public abstract class NodeCollectionForUniversalEdge : NodeCollection
        {
            [UnityEngine.SerializeField]
            List<ComplexMGNodes.UniversalEdge> nodes = new List<ComplexMGNodes.UniversalEdge>();

            public override IMGNode this[int index]
            {
                get
                {
                    return nodes[index];
                }
            }

            public override int Add()
            {
                nodes.Add(new ComplexMGNodes.UniversalEdge());
                return nodes.Count - 1;
            }

            public override void Remove(int index)
            {
                nodes.RemoveAt(index);
            }
            public override Type NodeType
            {
                get
                {
                    return typeof(ComplexMGNodes.UniversalEdge);
                }
            }
            public override int Count
            {
                get
                {
                    return nodes.Count;
                }
            }
            public override void CopyTo(NodeCollection target)
            {
                var _target = target as NodeCollectionForUniversalEdge;
                _target.nodes.Clear();
                _target.nodes.AddRange(nodes);
            }

        }

        [System.Serializable]
        public struct MapPare
        {
            public int typeCode;
            public int refIndex;
            public bool enabled;
        }

        public List<MapPare> map = new List<MapPare>();

        public IMGNode this[int index]
        {
            get
            {
                return GetNodeCollection(map[index].typeCode)[map[index].refIndex];
            }
        }

        public void Insert(int index, int typeCode)
        {
            var ncol = GetNodeCollection(typeCode);
            var i = ncol.Add();
            map.Insert(index, new MapPare() { refIndex = i, typeCode = typeCode, enabled = true });
        }
        public void Add(int typeCode)
        {
            var ncol = GetNodeCollection(typeCode);
            var i = ncol.Add();
            map.Add(new MapPare() { refIndex = i, typeCode = typeCode, enabled = true });
        }
        public void CopyTo(MGNodeSet target)
        {
            FillnodeTypeCollectionMap();
            target.FillnodeTypeCollectionMap();
            foreach (var item in nodeTypeCollectionMap)
            {
                item.Value.CopyTo(target.nodeTypeCollectionMap[item.Key]);
            }
            target.map.Clear();
            target.map.AddRange(map);
        }
        public void Remove(int index)
        {
            var refNode = map[index];
            map.RemoveAt(index);
            var nc = GetNodeCollection(refNode.typeCode);
            for (int i = 0; i < map.Count; i++)
            {
                var item = map[i];
                if (item.typeCode == refNode.typeCode && item.refIndex > refNode.refIndex)
                {
                    item.refIndex--;
                    map[i] = item;
                }
            }
            if (refNode.refIndex < nc.Count)
            {
                nc.Remove(refNode.refIndex);
            }
        }
        public void Clear()
        {
            var c = map.Count;
            for (int i = 0; i < c; i++)
            {
                Remove(0);
            }
        }

        public void Do(MeshGenerationContext contex)
        {
            for (int i = 0; i < map.Count; i++)
            {
                var item = map[i];
                if (item.enabled)
                {
                    var nd = GetNodeCollection(item.typeCode);
                    if (item.refIndex < nd.Count)
                    {
                        nd[item.refIndex].Do(contex);
                    }
                    else
                    {
                        // Debug.LogWarning("miss");
                    }

                }
            }
        }
        public NodeCollection GetNodeCollection(int typeCode)
        {
            FillnodeTypeCollectionMap();
            return nodeTypeCollectionMap[typeCode];
        }

        SortedList<int, NodeCollection> nodeTypeCollectionMap;
        private void FillnodeTypeCollectionMap()
        {
            if (nodeTypeCollectionMap != null) return;
            nodeTypeCollectionMap = new SortedList<int, NodeCollection>();
            var nodeList = new List<NodeCollection>();
            GetAllNodeCollections(nodeList);
            foreach (var item in nodeList)
            {
                nodeTypeCollectionMap.Add(item.TypeCode, item);
            }
        }

        public void GetAllNodeCollections(List<NodeCollection> res)
        {
            res.Add(nC_Vertex_AddEdge);
            res.Add(nC_Vertex_Extrude);
            res.Add(nC_Vertex_Offset);
            res.Add(nC_Vertex_Clone);
            res.Add(nC_Vertex_ByNormalExtrude);

            res.Add(nC_Triangle_Bridge);
            res.Add(nC_Triangle_Fill);

            res.Add(nC_Color_Paint);
            res.Add(nC_Color_ByNormalPaint);

            res.Add(nC_UV_Tille);
            res.Add(nC_UV_ByPath);

            res.Add(nC_Complex_UniversalEdge);
            res.Add(nC_Complex_Link);
        }

        public enum NodeTye : int
        {
            Vertex_AddEdge = 1000, Vertex_Extrude = 1001, Vertex_Offset = 1002, Vertex_Clone = 1003, Vertex_ByNormalExtrude = 1004,
            Triangle_Fill = 2000, Triangle_Bridge = 2001,
            Color_Paint = 3000, Color_ByNormalPaint = 3001,
            UV_Tille = 4000, UV_ByPath = 4001,
            Complex_UniversalEdge = 5000, Complex_Link = 5001
        }

        [System.Serializable]
        public class NC_Vertex_AddEdge : NodeCollection<VertexMGNodes.AddEdge>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Vertex_AddEdge;
                }
            }
        }
        [SerializeField]
        public NC_Vertex_AddEdge nC_Vertex_AddEdge;

        [System.Serializable]
        public class NC_Vertex_Extrude : NodeCollection<VertexMGNodes.Extrude>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Vertex_Extrude;
                }
            }
        }
        [SerializeField]
        public NC_Vertex_Extrude nC_Vertex_Extrude;

        [System.Serializable]
        public class NC_Vertex_ByNormalExtrude : NodeCollection<VertexMGNodes.ByNormalExtrude>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Vertex_ByNormalExtrude;
                }
            }
        }
        [SerializeField]
        public NC_Vertex_ByNormalExtrude nC_Vertex_ByNormalExtrude;


        [System.Serializable]
        public class NC_Vertex_Offset : NodeCollection<VertexMGNodes.Offset>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Vertex_Offset;
                }
            }
        }
        [SerializeField]
        public NC_Vertex_Offset nC_Vertex_Offset;


        [System.Serializable]
        public class NC_Triangle_Fill : NodeCollection<TriangleMGNodes.Fill>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Triangle_Fill;
                }
            }
        }
        [SerializeField]
        public NC_Triangle_Fill nC_Triangle_Fill;

        [System.Serializable]
        public class NC_Triangle_Bridge : NodeCollection<TriangleMGNodes.Bridge>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Triangle_Bridge;
                }
            }
        }
        [SerializeField]
        public NC_Triangle_Bridge nC_Triangle_Bridge;
        [System.Serializable]
        public class NC_Color_Paint : NodeCollection<ColorMGNodes.Paint>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Color_Paint;
                }
            }
        }
        [SerializeField]
        public NC_Color_Paint nC_Color_Paint;
        [System.Serializable]
        public class NC_Color_ByNormalPaint : NodeCollection<ColorMGNodes.ByNormalPaint>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Color_ByNormalPaint;
                }
            }
        }
        [SerializeField]
        public NC_Color_ByNormalPaint nC_Color_ByNormalPaint;

        [System.Serializable]
        public class NC_UVMGNodes_Tille : NodeCollection<UVMGNodes.Tille>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.UV_Tille;
                }
            }
        }
        [SerializeField]
        public NC_UVMGNodes_Tille nC_UV_Tille;

        [System.Serializable]
        public class NC_Complex_UniversalEdge : NodeCollectionForUniversalEdge
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Complex_UniversalEdge;
                }
            }
        }
        [SerializeField]
        public NC_Complex_UniversalEdge nC_Complex_UniversalEdge;

        [System.Serializable]
        public class NC_Vertex_Clone : NodeCollection<VertexMGNodes.Clone>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Vertex_Clone;
                }
            }
        }
        [SerializeField]
        public NC_Vertex_Clone nC_Vertex_Clone;

        [System.Serializable]
        public class NC_Complex_Link : NodeCollection<ComplexMGNodes.Link>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.Complex_Link;
                }
            }
        }
        [SerializeField]
        public NC_Complex_Link nC_Complex_Link;

        [System.Serializable]
        public class NC_UV_ByPath : NodeCollection<UVMGNodes.ByPath>
        {
            public override int TypeCode
            {
                get
                {
                    return (int)NodeTye.UV_ByPath;
                }
            }
        }
        [SerializeField]
        public NC_UV_ByPath nC_UV_ByPath = new NC_UV_ByPath();
    }
}

