using System;

using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

namespace Kiskovi.Core
{
    internal class InputTrigger : MonoBehaviour
    {
        [Inject] private InputActionReference inputActionRef;

        public Type type;
        public TriggerAction onPressed;

        private InputAction _inputAction;

        private void Awake()
        {
            _inputAction = inputActionRef.action;
            _inputAction.Enable();
        }

        private void OnEnable()
        {
            _inputAction.performed += _inputAction_performed;
        }

        private void OnDisable()
        {
            _inputAction.performed -= _inputAction_performed;
        }

        private void _inputAction_performed(InputAction.CallbackContext obj)
        {
            OnActionPressed();
        }

        private void OnActionPressed()
        {
            TriggerAction.Trigger(onPressed);
        }
    }
}
