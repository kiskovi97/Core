using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    internal class ActionTrigger : MonoBehaviour
    {
        public enum Type
        {
            Action,
            Cancel
        }

        [Inject] private SignalBus _signalBus;

        public Type type;
        public TriggerAction onPressed;

        private void OnEnable()
        {
            switch(type)
            {
                case Type.Action:
                    _signalBus.Subscribe<ActionPressedSignal>(OnActionPressed);
                    break;
                case Type.Cancel:
                    _signalBus.Subscribe<CancelPressedSignal>(OnActionPressed);
                    break;
            }
        }

        private void OnDisable()
        {
            switch (type)
            {
                case Type.Action:
                    _signalBus.TryUnsubscribe<ActionPressedSignal>(OnActionPressed);
                    break;
                case Type.Cancel:
                    _signalBus.TryUnsubscribe<CancelPressedSignal>(OnActionPressed);
                    break;
            }
        }

        private void OnActionPressed()
        {
            TriggerAction.Trigger(onPressed);
        }
    }
}
