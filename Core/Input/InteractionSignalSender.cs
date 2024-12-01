using UnityEngine.InputSystem;

using Zenject;

using static Kiskovi.Core.BasicInputActions;

namespace Kiskovi.Core
{
    public class ActionPressedSignal { }

    public class ActionHoldSignal
    {
        public bool isHolding;
        public ActionHoldSignal(bool isHolding)
        {
            this.isHolding = isHolding;
        }
    }
    public class CancelPressedSignal { }

    internal class InteractionSignalSender : IInteractionActions
    {
        private SignalBus _signalBus;

        public InteractionSignalSender(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _signalBus.TryFire(new ActionPressedSignal());
            }
        }

        public void OnActionHold(InputAction.CallbackContext context)
        {
            _signalBus.TryFire(new ActionHoldSignal(context.performed));
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _signalBus.TryFire(new CancelPressedSignal());
            }
        }
    }
}
