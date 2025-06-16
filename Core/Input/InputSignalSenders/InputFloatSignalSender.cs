using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    internal class InputFloatSignalSender : InputSignalSender
    {
        protected override void Subscirbe()
        {
            actionReference.action.performed += OnPerformed;
            actionReference.action.canceled += OnPerformed;
            actionReference.action.started += OnPerformed;
        }

        protected override void UnSubscirbe()
        {
            actionReference.action.performed -= OnPerformed;
            actionReference.action.canceled -= OnPerformed;
            actionReference.action.started -= OnPerformed;
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            if (CachedType != null && typeof(InputFloatSignal).IsAssignableFrom(CachedType))
            {
                var instance = Activator.CreateInstance(CachedType, context.ReadValue<float>());
                _signalBus.TryFire(instance);
            }
            else
            {
                Debug.LogWarning("Invalid or null signal type");
            }
        }
    }
}
