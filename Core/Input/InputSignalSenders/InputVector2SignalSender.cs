using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    internal class InputVector2SignalSender : InputSignalSender
    {
        public bool onlyPerformend = false;
        public bool onlyGreaterThenZero = false;
        public bool normalized = false;

        protected override void Subscirbe()
        {
            actionReference.action.performed += OnPerformed;
            if (!onlyPerformend)
            {
                actionReference.action.canceled += OnPerformed;
                actionReference.action.started += OnPerformed;
            }
        }

        protected override void UnSubscirbe()
        {
            actionReference.action.performed -= OnPerformed;

            if (!onlyPerformend)
            {
                actionReference.action.canceled -= OnPerformed;
                actionReference.action.started -= OnPerformed;
            }
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();

            if (onlyGreaterThenZero && value.magnitude == 0) return;

            if (normalized)
                value = value.normalized;

            if (CachedType != null && typeof(InputVector2Signal).IsAssignableFrom(CachedType))
            {
                var instance = Activator.CreateInstance(CachedType, value);
                _signalBus.TryFire(instance);
            }
            else
            {
                Debug.LogWarning("Invalid or null signal type");
            }
        }
    }
}
