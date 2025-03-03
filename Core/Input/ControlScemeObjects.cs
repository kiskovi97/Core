using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class ControlScemeObjects : MonoBehaviour
    {
        public GameObject keyboardObject;
        public GameObject xboxObject;

        [Inject] private SignalBus _signalBus;

        private void Start()
        {
            SetObjects(InputSignals.Scheme);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<InputSignals.ControlSchemeChanged>(OnControlSchemeChanged);
            SetObjects(InputSignals.Scheme);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<InputSignals.ControlSchemeChanged>(OnControlSchemeChanged);
        }

        private void OnControlSchemeChanged(InputSignals.ControlSchemeChanged obj)
        {
            SetObjects(obj.Scheme);
        }
        private void SetObjects(ControlScheme Scheme)
        {
            switch (Scheme)
            {
                case ControlScheme.Keyboard:
                    if (keyboardObject != null)
                        keyboardObject.SetActive(true);
                    if (xboxObject != null)
                        xboxObject.SetActive(false);
                    break;
                case ControlScheme.XboxController:
                    if (keyboardObject != null)
                        keyboardObject.SetActive(false);
                    if (xboxObject != null)
                        xboxObject.SetActive(true);
                    break;
            }
        }
    }
}
