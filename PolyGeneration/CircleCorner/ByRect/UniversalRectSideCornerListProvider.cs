using System.Collections.Generic;
using UnityEngine;
using UShape.Libs;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UShape.PolyGeneration.CircleCorner.ByRect
{
    [System.Serializable]
    public class UniversalRectSideCornerListProvider : IRectSideCornerListProvider
    {
        [SerializeField]
        private InterfaceComponent_IRectSideCornerListProvider externalSideCornerList = new InterfaceComponent_IRectSideCornerListProvider();
        [SerializeField]
        private List<RectSideCornerPara> sideCornerList = new List<RectSideCornerPara>();

        public List<RectSideCornerPara> SideCornerList
        {
            get
            {
                var uo = externalSideCornerList.UsedObject;
                if (uo != null)
                {
                    return uo.SideCornerList;
                }
                return sideCornerList;
            }
        }
        public void ResetTo2CornersHorizontal(int circleSegmentCount = 30)
        {
            var sideCornerList = SideCornerList;
            sideCornerList.Clear();
            sideCornerList.Add(new RectSideCornerPara()
            {
                corner = new RectCorner()
                {
                    position = new AnchorPoint()
                    {
                        anchor = new Vector2(0, 0.5f)
                    },
                    diameterRatio = 1,
                    segmentCount = circleSegmentCount,
                    orenatiotion = RectCorner.Orenatiotion.Horizontal
                },
                side = new RectSide()
                {
                    p1 = new AnchorPoint()
                    {
                        anchor = new Vector2(1f / 3f, 0),
                        offset = new Vector2(0, -1f)
                    },
                    p2 = new AnchorPoint()
                    {
                        anchor = new Vector2(1f / 3f * 2f, 0),
                        offset = new Vector2(0, -1f)
                    },
                    mode = SideMode.Flat,
                    segmentCount = 30,
                }
            });
            sideCornerList.Add(new RectSideCornerPara()
            {
                corner = new RectCorner()
                {
                    position = new AnchorPoint()
                    {
                        anchor = new Vector2(1, 0.5f)
                    },
                    diameterRatio = 1,
                    segmentCount = circleSegmentCount
                },
                side = new RectSide()
                {
                    p1 = new AnchorPoint()
                    {
                        anchor = new Vector2(1f / 3f * 2f, 1),
                        offset = new Vector2(0, -1f)
                    },
                    p2 = new AnchorPoint()
                    {
                        anchor = new Vector2(1f / 3f, 1),
                        offset = new Vector2(0, -1f)
                    },
                    mode = SideMode.Flat,
                    segmentCount = circleSegmentCount
                }

            });
        }

        public void ResetTo4CornersHorizontal(int circleSegmentCount = 30, float circleRadius = 30 )
        {

            var corner1 = new RectCorner()
            {
                position = new AnchorPoint()
                {
                    anchor = Vector2.zero,
                    offset = new Vector2(circleRadius, circleRadius)
                },
                diameterRatio = 0,
                diameterOffset = circleRadius * 2f,
                segmentCount = circleSegmentCount,
                orenatiotion = RectCorner.Orenatiotion.Horizontal
            };
            var side1 = new RectSide()
            {
                p1 = new AnchorPoint()
                {
                    anchor = new Vector2(1f / 3f, 0),
                    offset = new Vector2(0, -1f)
                },
                p2 = new AnchorPoint()
                {
                    anchor = new Vector2(1f / 3f * 2f, 0),
                    offset = new Vector2(0, -1f)
                },
                mode = SideMode.Flat,
                segmentCount = 30
            };
            var corner2 = new RectCorner()
            {
                position = new AnchorPoint()
                {
                    anchor = new Vector2(1, 0),
                    offset = new Vector2(-circleRadius, circleRadius)
                },
                diameterRatio = 0,
                diameterOffset = circleRadius * 2f,
                segmentCount = circleSegmentCount
            };
            var side2 = new RectSide()
            {
                p1 = new AnchorPoint()
                {
                    anchor = new Vector2(1, 1f / 3f),
                    offset = new Vector2(0, -1f)
                },
                p2 = new AnchorPoint()
                {
                    anchor = new Vector2(1, 1f / 3f * 2f),
                    offset = new Vector2(0, -1f)
                },
                mode = SideMode.Flat,
                segmentCount = 30
            };

            var corner3 = new RectCorner()
            {
                position = new AnchorPoint()
                {
                    anchor = new Vector2(1, 1),
                    offset =  new Vector2(-circleRadius, -circleRadius)
                },
                diameterRatio = 0,
                diameterOffset = circleRadius * 2f,
                segmentCount = circleSegmentCount
            };
            var side3 = new RectSide()
            {
                p1 = new AnchorPoint()
                {
                    anchor = new Vector2(1f / 3f * 2f, 1),
                    offset = new Vector2(-1f, 0)
                },
                p2 = new AnchorPoint()
                {
                    anchor = new Vector2(1f / 3f, 1),
                    offset = new Vector2(-1f, 0)
                },
                mode = SideMode.Flat,
                segmentCount = 30
            };

            var corner4 = new RectCorner()
            {
                position = new AnchorPoint()
                {
                    anchor = new Vector2(0, 1f),
                    offset = new Vector2(circleRadius, -circleRadius)
                },
                diameterRatio = 0,
                diameterOffset = circleRadius * 2f,
                segmentCount = circleSegmentCount,
                orenatiotion = RectCorner.Orenatiotion.Horizontal
            };

            var side4 = new RectSide()
            {
                p1 = new AnchorPoint()
                {
                    anchor = new Vector2(0, 1f / 3f * 2f),
                    offset = new Vector2(0, 1f)
                },
                p2 = new AnchorPoint()
                {
                    anchor = new Vector2(0, 1f / 3f),
                    offset = new Vector2(0, 1f)
                },
                mode = SideMode.Flat,
                segmentCount = 30
            };
            var sideCornerList = this.SideCornerList;

            sideCornerList.Clear();
            sideCornerList.Add(new RectSideCornerPara() { corner = corner1, side = side1 });
            sideCornerList.Add(new RectSideCornerPara() { corner = corner2, side = side2 });
            sideCornerList.Add(new RectSideCornerPara() { corner = corner3, side = side3 });
            sideCornerList.Add(new RectSideCornerPara() { corner = corner4, side = side4 });
        }

    }
#if UNITY_EDITOR
    public class UniversalRectSideCornerListProviderProppertyDrawer
    {
        SerializedProperty property;
        SimpleReorderableList rList;
        public UniversalRectSideCornerListProviderProppertyDrawer(SerializedProperty property)
        {
            this.property = property;
            rList = new SimpleReorderableList(property.serializedObject, property.FindPropertyRelative("sideCornerList"));
        }
        public void DoLayout()
        {
            var externalListProp = property.FindPropertyRelative("externalSideCornerList");
            EditorGUILayout.PropertyField(externalListProp);
            if (!(externalListProp.FindPropertyRelative("externalObject").objectReferenceValue is IRectSideCornerListProvider))
            {
                rList.DoLayoutList();
            }
        }
    }
#endif
}
