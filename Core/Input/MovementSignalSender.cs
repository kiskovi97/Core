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

    internal class MovementSignalSender : IMovementActions
    {
        private SignalBus _signalBus;

        public MovementSignalSender(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _signalBus.Fire(new MoveSignal(context.ReadValue<Vector2>()));
        }
    }
}
