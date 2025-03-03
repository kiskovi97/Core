using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kiskovi.Core
{
    public class SettingsVolume : MonoBehaviour
    {
        [Inject] private ISettingsTable settings;

        public enum Type { Music, Sound }
        public Type type;
        public Slider slider;

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }
        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        void Awake()
        {
            switch(type)
            {
                case Type.Music:
                    slider.value = settings.MusicVolume;
                    break;
                case Type.Sound:
                    slider.value = settings.SoundVolume;
                    break;
            }
        }

        private void OnValueChanged(float newValue)
        {
            switch (type)
            {
                case Type.Music:
                    settings.MusicVolume = slider.value;
                    break;
                case Type.Sound:
                    settings.SoundVolume = slider.value;
                    break;
            }
        }
    }
}
