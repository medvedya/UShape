using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UShape.Libs;
namespace UShape.MeshGeneration.Rendering
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class UIShapeRenderer : MaskableGraphic, IMGNode
    {
        [SerializeField]
        private InterfaceObject_IPolyShapeProvider polyShapeProvider = new InterfaceObject_IPolyShapeProvider();

        [SerializeField] Texture m_Texture;

        public override Texture mainTexture
        {
            get
            {
                if (m_Texture == null)
                {
                    if (material != null && material.mainTexture != null)
                    {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return m_Texture;
            }
        }
        public Texture Texture
        {
            get
            {
                return m_Texture;
            }
            set
            {
                if (m_Texture == value)
                    return;

                m_Texture = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }
        public MGNodeSet nodeSet;

        PolyShape lastPolyShape;

        bool needGenerateMesh = false;
        void Draw(IPolyShapeProvider polyShapeProvider)
        {
            lastPolyShape = polyShapeProvider.Poly;
            needGenerateMesh = true;
            SetVerticesDirty();

        }
        public float extrude;
        protected UIShapeRenderer()
        {
            useLegacyMeshGeneration = false;
        }

        MeshGenerationContext contex = new MeshGenerationContext();
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (lastPolyShape == null)
            {
                return;
            }
            if (needGenerateMesh)
            {
                contex.Clear();
                contex.polyShape = lastPolyShape;
                contex.mesh.defaultColor = this.color;
                nodeSet.Do(contex);
                needGenerateMesh = false;
            }

            var mesh = contex.mesh;
            for (int i = 0; i < contex.mesh.vertexes.Count; i++)
            {
                vh.AddVert(mesh.vertexes[i], mesh.GetColor(i), mesh.GetUV(i));
            }
            for (int i = 0; i < mesh.trises.Count; i += 3)
            {
                vh.AddTriangle(mesh.trises[i], mesh.trises[i + 1], mesh.trises[i + 2]);
            }
        }

        public void Do(MeshGenerationContext contex)
        {
            nodeSet.Do(contex);
        }
        IPolyShapeProvider old;
        private void Update()
        {
            if (old != polyShapeProvider.UsedObject)
            {
                OnPolyShapeProviderChange(old, polyShapeProvider.UsedObject);
                old = polyShapeProvider.UsedObject;
            }
        }
        public void OnPolyShapeProviderChange(IPolyShapeProvider oldObj, IPolyShapeProvider newObj)
        {
            if (oldObj != null)
            {
                oldObj.OnPolyShapeChange -= Draw;
            }
            if (newObj != null)
            {
                newObj.OnPolyShapeChange += Draw;
                lastPolyShape = newObj.Poly;
            }
            else
            {
                lastPolyShape = null;
            }
            needGenerateMesh = true;
            SetVerticesDirty();
        }
        public void SetMeshAsDirty()
        {
            needGenerateMesh = true;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnPolyShapeProviderChange(old, null);

        }
    }
}