using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    internal class InputFloatToBoolSignalSender : InputSignalSender
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
            if (CachedType != null && typeof(InputBooleanSignal).IsAssignableFrom(CachedType))
            {
                var value = context.ReadValue<float>();
                if (value != 0)
                {
                    var boolValue = value > 0;
                    var instance = Activator.CreateInstance(CachedType, args: boolValue);
                    _signalBus.TryFire(instance);
                }
            }
            else
            {
                Debug.LogWarning("Invalid or null signal type");
            }
        }
    }
}
