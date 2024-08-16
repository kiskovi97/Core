using System.Collections.Generic;
using System.Linq;

using ModestTree;

using UnityEngine;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    public class GraphLines : Graphic
    {
        public List<List<float>> _lines = new List<List<float>>()
        {
            new List<float>() { 0.2f,0.4f, 1f, },
            new List<float>() { 1f,0.4f, 0f, },
        };
        private List<float> _mainLine = new List<float>()
        {
            1f,0.4f, 0f,
        };
        public Color32 otherColor;
        public float lineSize = 2f;
        public float lineSizeSmall = 0.5f;

        public void SetLines(IEnumerable<List<float>> lines)
        {
            this._lines = new List<List<float>>();
            foreach(var line in lines)
            {
                if (line.Count > 10)
                {
                    var list = new List<float>();
                    var step = line.Count / 10 + 1;
                    var index = 0;
                    while(line.Skip(index).Take(step).Any())
                    {
                        var avg = line.Skip(index).Take(step).Average();
                        list.Add(avg);
                        index += step;
                    }
                    this._lines.Add(list.ToList());
                } else
                {
                    this._lines.Add(line.ToList());
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
        public void SetMainLine(List<float> mainLine)
        {
            if (mainLine.Count > 10)
            {
                var list = new List<float>();
                var step = mainLine.Count / 10 + 1;
                var index = 0;
                while (mainLine.Skip(index).Take(step).Any())
                {
                    var avg = mainLine.Skip(index).Take(step).Average();
                    list.Add(avg);
                    index += step;
                }
                this._mainLine = list.ToList();
            }
            else
            {
                this._mainLine = mainLine.ToList();
            }

            var original = gameObject.activeSelf;
            gameObject.SetActive(false);

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);

            gameObject.SetActive(original);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var rect = GetPixelAdjustedRect();

            vh.Clear();

#if UNITY_WEBGL
            foreach (var line in lines.Take(20))
#else
            foreach (var line in _lines)
#endif
            {
                var length = line.Count - 1f;
                for (int i = 1; i < line.Count; i++)
                {
                    var value = line[i];
                    var valuePrev = line[i - 1];
#if UNITY_WEBGL
                    DrawLine(vh, GetPosition((i - 1) / length, valuePrev, rect), GetPosition(i / length, value, rect), otherColor, lineSizeSmall * 6f);
#else
                    DrawLine(vh, GetPosition((i - 1) / length, valuePrev, rect), GetPosition(i / length, value, rect), otherColor, lineSizeSmall);
#endif
                }
            }

            if (_mainLine == null) return;

            var mainLength = _mainLine.Count - 1f;
            for (int i = 1; i < _mainLine.Count; i++)
            {
                var value = _mainLine[i];
                var valuePrev = _mainLine[i - 1];
                DrawLine(vh, GetPosition((i - 1) / mainLength, valuePrev, rect), GetPosition(i / mainLength, value, rect), color, lineSize);
            }

        }

        protected Vector2 GetPosition(float x, float y, Rect rect)
        {
            return new Vector2(rect.x + rect.width * x, rect.y + rect.height * y);
        }

        protected void DrawLine(VertexHelper vh, Vector2 from, Vector2 to, Color color, float lineSize)
        {
            var index = vh.currentVertCount;

            vh.AddVert(from, color, new Vector2(0f, 0f));
            vh.AddVert(from + Vector2.down * lineSize, color, new Vector2(0f, 0f));
            vh.AddVert(to + Vector2.down * lineSize, color, new Vector2(0f, 0f));
            vh.AddVert(to, color, new Vector2(0f, 0f));

            vh.AddTriangle(index, index + 1, index + 2);
            vh.AddTriangle(index + 2, index + 3, index);
        }
    }
}
