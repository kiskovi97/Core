using UnityEngine;

namespace Kiskovi.Core
{
    internal class PauseMenu : MonoBehaviour
    {
        public UIWindow window;

        private void Awake()
        {
            UIWindow.PauseMenu = window;
        }
    }
}
