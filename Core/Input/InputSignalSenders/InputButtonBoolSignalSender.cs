using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    internal class InputButtonBoolSignalSender : InputSignalSender
    {
        protected override void Subscirbe()
        {
            actionReference.action.performed += OnPerformed;
            actionReference.action.canceled += OnCanceled;
        }

        protected override void UnSubscirbe()
        {
            actionReference.action.performed -= OnPerformed;
            actionReference.action.canceled -= OnCanceled;
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            if (CachedType != null && typeof(InputBooleanSignal).IsAssignableFrom(CachedType))
            {
                var instance = Activator.CreateInstance(CachedType, args: true);
                _signalBus.TryFire(instance);
            }
            else
            {
                Debug.LogWarning("Invalid or null signal type");
            }
        }

        private void OnCanceled(InputAction.CallbackContext context)
        {
            if (CachedType != null && typeof(InputBooleanSignal).IsAssignableFrom(CachedType))
            {
                var instance = Activator.CreateInstance(CachedType, args: false);
                _signalBus.TryFire(instance);
            }
            else
            {
                Debug.LogWarning("Invalid or null signal type");
            }
        }
    }
}
