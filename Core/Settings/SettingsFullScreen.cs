using UnityEngine;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    internal class SettingsFullScreen : MonoBehaviour
    {
        [SerializeField]
        private Toggle toggle;
        private bool isInitialized;

        void OnEnable()
        {
            toggle.isOn = Screen.fullScreen;
            toggle.onValueChanged.AddListener(OnValueChanged);
            isInitialized = true;
        }

        void OnDisable()
        {
            isInitialized = false;
            toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool newValue)
        {
            if (!isInitialized)
                return;
            Screen.fullScreen = newValue;
        }
    }
}
