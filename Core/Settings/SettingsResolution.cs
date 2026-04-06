using System;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Dropdown;

namespace Kiskovi.Core
{
    internal class SettingsResolution : MonoBehaviour
    {
        [SerializeField]
        private SelectionBox selectionBox;

        void Awake()
        {
#if !(UNITY_ANDROID || UNITY_IOS)
            if (selectionBox != null)
                selectionBox.onValueChanged.AddListener(OnValueChanged);
#else
            Destroy(gameObject);
#endif
        }

#if !(UNITY_ANDROID || UNITY_IOS)

        private int resCount = 0;

        private void Update()
        {
            selectionBox.interactable = Screen.fullScreen;
            if (resCount != Screen.resolutions.Length)
            {
                Initialized();
            }
        }

        void OnEnable()
        {
            Initialized();
        }

        private void Initialized()
        {
            resCount = Screen.resolutions.Length;
            var options = Screen.resolutions.Select(item => new OptionData(
                item.width + " x " + item.height
            ));
            var value = Screen.currentResolution;
            if (Screen.resolutions.Contains(value))
            {
                var index = Array.IndexOf(Screen.resolutions, value);
                selectionBox.SetOptions(options, index);
            }
            else
            {
                selectionBox.SetOptions(options, Screen.resolutions.Length - 1);
            }
        }

        private void OnValueChanged(int newValue)
        {
            var value = Screen.resolutions[newValue];
            Screen.SetResolution(value.width, value.height, Screen.fullScreen);
        }
#endif
    }
}
