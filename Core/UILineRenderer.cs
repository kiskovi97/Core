using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    [System.Serializable]
    public class Line
    {
        public Vector2 startPoint;
        public Vector2 endPoint;
        public Line(Vector2 start, Vector2 end)
        {
            startPoint = start;
            endPoint = end;
        }
    }

    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : Graphic
    {
        [SerializeField] private float lineWidth = 5f;

        private List<Line> _lines = new List<Line>();

        public void SetLine(Vector2 start, Vector2 end)
        {
            _lines.Clear();
            _lines.Add(new Line(rectTransform.InverseTransformPoint(start), rectTransform.InverseTransformPoint(end)));
            SetVerticesDirty();
        }
        public void SetLines(IEnumerable<Line> lines)
        {
            _lines = lines.Select(item => new Line(
                rectTransform.InverseTransformPoint(item.startPoint),
                rectTransform.InverseTransformPoint(item.endPoint)
            )).ToList();
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            int index = 0;
            foreach (var line in _lines)
            {
                Vector2 direction = (line.endPoint - line.startPoint).normalized;
                Vector2 normal = new Vector2(-direction.y, direction.x) * (lineWidth * 0.5f);

                Vector2 v1 = line.startPoint + normal;
                Vector2 v2 = line.startPoint - normal;
                Vector2 v3 = line.endPoint - normal;
                Vector2 v4 = line.endPoint + normal;


                vh.AddVert(v1, color, Vector2.zero);
                vh.AddVert(v2, color, Vector2.zero);
                vh.AddVert(v3, color, Vector2.zero);
                vh.AddVert(v4, color, Vector2.zero);

                vh.AddTriangle(index + 0, index + 1, index + 2);
                vh.AddTriangle(index + 2, index + 3, index + 0);
                index += 4;
            }
        }

        public void ResetLine()
        {
            _lines.Clear();
            SetVerticesDirty();
        }
    }
}
