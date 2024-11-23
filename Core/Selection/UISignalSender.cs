using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

using static Kiskovi.Core.BasicInputActions;

namespace Kiskovi.Core
{
    internal class UISignalSender : IUIInputsActions
    {
        private SignalBus _signalBus;

        public UISignalSender(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnAccept(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _signalBus.TryFire(new UIInteractions.AccepSignal());
            }
        }

        public void OnDecline(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _signalBus.TryFire(new UIInteractions.DeclineSignal());
            }
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var current = context.ReadValue<Vector2>();
                if (current.magnitude > 0f)
                {
                    _signalBus.TryFire(new UIInteractions.Navigate(current.normalized));
                }
            }
        }

        public void OnNavigateUI(InputAction.CallbackContext context)
        {
            if (context.performed && Time.deltaTime < 0.1f)
            {
                var current = context.ReadValue<Vector2>();
                if (current.magnitude > 0f)
                {
                    _signalBus.TryFire(new UIInteractions.NavigateUI(current.normalized));
                }
            }
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _signalBus.TryFire(new UIInteractions.ExitSignal());
            }
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _signalBus.TryFire(new UIInteractions.NavigateTabsSignal(true));
            }
        }

        public void OnPrev(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _signalBus.TryFire(new UIInteractions.NavigateTabsSignal(false));
            }
        }

        public void OnDelete(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _signalBus.TryFire(new UIInteractions.DeleteSignal());
            }
        }

        public void OnModifyValue(InputAction.CallbackContext context)
        {
            _signalBus.TryFire(new UIInteractions.ModifyValueSignal(context.ReadValue<float>()));
        }
    }
}
