using UnityEngine;

namespace Kiskovi.Core
{
    public class Colorize : MonoBehaviour
    {
        public SpriteRenderer[] renderers;
        public LineRenderer[] lines;
        public Color color;

        public void OnEnable()
        {
            foreach (var renderer in renderers)
            {
                renderer.color = color;
            }

            foreach (var renderer in lines)
            {
                var gradient = renderer.colorGradient;
                gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
                renderer.colorGradient = gradient;
            }
        }
    }
}
