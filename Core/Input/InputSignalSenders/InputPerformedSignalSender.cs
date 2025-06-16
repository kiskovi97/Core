using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    internal class InputPerformedSignalSender : InputSignalSender
    {
        protected override void Subscirbe()
        {
            actionReference.action.performed += OnPerformed;
        }

        protected override void UnSubscirbe()
        {
            actionReference.action.performed -= OnPerformed;
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            if (CachedType != null && typeof(InputSimpleSignal).IsAssignableFrom(CachedType))
            {
                var instance = Activator.CreateInstance(CachedType);
                _signalBus.TryFire(instance);
            }
            else
            {
                Debug.LogWarning("Invalid or null signal type: " + name);
            }
        }
    }
}
