using UnityEngine;
namespace UShape.MeshGeneration.Rendering
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class MeshShapeRenderer:MonoBehaviour
    {
        [SerializeField]
        private InterfaceObject_IPolyShapeProvider polyShapeProvider = new InterfaceObject_IPolyShapeProvider();
        public MGNodeSet nodeSet = new MGNodeSet();
        public Color color = Color.white;
        IPolyShapeProvider currentShapeProvider;
        Mesh mesh;
        MeshFilter meshFilter;
        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            mesh = new Mesh();
            mesh.MarkDynamic();
            OnPolyShapeProviderChange(null, currentShapeProvider);
            meshFilter.sharedMesh = mesh;
        }
        private void Update()
        {
            if (currentShapeProvider != polyShapeProvider.UsedObject)
            {
                OnPolyShapeProviderChange(currentShapeProvider, polyShapeProvider.UsedObject);
                currentShapeProvider = polyShapeProvider.UsedObject;
            }
        }
        public void OnPolyShapeProviderChange(IPolyShapeProvider oldObj, IPolyShapeProvider newObj)
        {
            currentShapeProvider = newObj;
            if (oldObj != null)
            {
                oldObj.OnPolyShapeChange -= OnDraw;
            }
            if (newObj != null)
            {
                newObj.OnPolyShapeChange += OnDraw;
                Draw();
            }
        }
        void OnDestroy()
        {
            if (Application.isPlaying)
            {
                Destroy(mesh);
            }
            else
            {
                DestroyImmediate(mesh);
            }
        }
        MeshGenerationContext contex = new MeshGenerationContext();
        void OnDraw(IPolyShapeProvider pp)
        {
            Draw();
        }
        public void Draw()
        {
            if (currentShapeProvider == null) return;
            contex.Clear();
            contex.mesh.defaultColor = color;
            contex.polyShape = currentShapeProvider.Poly;
            nodeSet.Do(contex);
            mesh.Clear();
            mesh.SetVertices(contex.mesh.vertexes);
            mesh.SetColors(contex.mesh.colors);
            mesh.SetUVs(0, contex.mesh.uv);
            mesh.SetTriangles(contex.mesh.triangles, 0);
        }
    }
}