using UnityEngine;

namespace Kiskovi.Core
{
    public class ColorizeGroup: MonoBehaviour
    {
        public Color color;
        public void OnEnable()
        {
            var colorizeChildren = GetComponentsInChildren<Colorize>(true);
            foreach(var colorize in colorizeChildren)
            {
                colorize.color = color;
                colorize.OnEnable();
            }
        }
    }
}
