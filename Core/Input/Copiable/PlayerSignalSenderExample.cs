using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

using static Kiskovi.Core.InputActionsExample;

namespace Kiskovi.Core
{

    internal class PlayerSignalSenderExample : IPlayerActions
    {
        private SignalBus _signalBus;

        public PlayerSignalSenderExample(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _signalBus.Fire(new MoveSignal(context.ReadValue<Vector2>()));
        }
    }
}
