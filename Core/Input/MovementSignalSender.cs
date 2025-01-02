using UnityEngine;
using UnityEngine.InputSystem;

using static Kiskovi.Core.BasicInputActions;

using Zenject;

namespace Kiskovi.Core
{
    public class MoveSignal
    {
        public Vector2 Move;

        public MoveSignal(Vector2 move)
        {
            Move = move;
        }
    }
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

    internal class PlayerSignalSender : IPlayerActions
    {
        private SignalBus _signalBus;

        public PlayerSignalSender(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _signalBus.Fire(new MoveSignal(context.ReadValue<Vector2>()));
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
