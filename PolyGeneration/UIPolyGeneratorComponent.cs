using System;
using UnityEngine;
namespace UShape.PolyGeneration
{
    [ExecuteInEditMode]
    public abstract class UIPolyGeneratorComponent : MonoBehaviour, IPolyShapeProvider
    {
        PolyShape polyShape = new PolyShape();
        public PolyShape Poly
        {
            get
            {
                return polyShape;
            }
        }
        public event Action<IPolyShapeProvider> OnPolyShapeChange;

        RectTransform _rectransform;
        RectTransform RectTransform
        {
            get
            {
                if (_rectransform == null)
                {
                    _rectransform = GetComponent<RectTransform>();
                }
                return _rectransform;

            }
        }
        private void Awake()
        {
            Generate();
        }
        void OnRectTransformDimensionsChange()
        {
            Generate();
        }
        void OnValidate()
        {
            Generate();
        }
        private void OnAnimatorMove()
        {
            Generate();
        }
        public void Generate()
        {
            if (OnGenerate(Poly, RectTransform.rect))
            {
                if (OnPolyShapeChange != null)
                {
                    OnPolyShapeChange(this);
                }
            }
        }
        protected abstract bool OnGenerate(PolyShape polyShape, Rect rect);
    }
}
