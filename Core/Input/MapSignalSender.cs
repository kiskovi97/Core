using UnityEngine.InputSystem;

using Zenject;

using static Kiskovi.Core.BasicInputActions;

namespace Kiskovi.Core
{
    public class ToggleMapSignal { }
    internal class MapSignalSender : IMapActions
    {
        private SignalBus _signalBus;

        public MapSignalSender(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnToggleMap(InputAction.CallbackContext context)
        {
            if (context.performed)
                _signalBus.TryFire(new ToggleMapSignal());
        }
    }
}
