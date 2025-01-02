using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

using static Kiskovi.Core.BasicInputActions;

namespace Kiskovi.Core
{
    internal class PlayerSignalSenderBehaviour : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject(Id = "PlayerId")] private string _id;

        public void OnMove(InputValue context)
        {
            _signalBus.TryFireId(_id, new MoveSignal(context.Get<Vector2>()));
        }

        public void OnAction()
        {
            _signalBus.TryFireId(_id, new ActionPressedSignal());
        }

        public void OnActionHold(InputValue context)
        {
            _signalBus.TryFireId(_id, new ActionHoldSignal(context.isPressed));
        }

        public void OnCancel()
        {
            _signalBus.TryFireId(_id, new CancelPressedSignal());
        }
    }
}
