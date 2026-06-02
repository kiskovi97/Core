using UnityEngine;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    internal class SettingsFullScreen : MonoBehaviour
    {
        [SerializeField]
        private Toggle toggle;

        void OnEnable()
        {
            toggle.isOn = Screen.fullScreen;
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        void OnDisable()
        {
            toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool newValue)
        {
            Screen.fullScreen = newValue;
        }
    }
}
