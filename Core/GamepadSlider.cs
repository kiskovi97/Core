using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kiskovi.Core
{ 
    internal class GamepadSlider : MonoBehaviour
    {
        public Slider slider;

        [Inject] private SignalBus _signalBus;

        private void OnEnable()
        {
            _signalBus.Subscribe<UIInteractions.ModifyValueSignal>(OnModifyValue);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<UIInteractions.ModifyValueSignal>(OnModifyValue);
        }

        private void OnModifyValue(UIInteractions.ModifyValueSignal signal)
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == slider.gameObject)
                slider.value += signal.Delta * 0.1f;
        }
    }
}
