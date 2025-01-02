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
        [Inject(Id = "PlayerId")] private string _id;

        public Type type;
        public TriggerAction onPressed;

        private void OnEnable()
        {
            switch(type)
            {
                case Type.Action:
                    _signalBus.SubscribeId<ActionPressedSignal>(_id, OnActionPressed);
                    break;
                case Type.Cancel:
                    _signalBus.SubscribeId<CancelPressedSignal>(_id, OnActionPressed);
                    break;
            }
        }

        private void OnDisable()
        {
            switch (type)
            {
                case Type.Action:
                    _signalBus.TryUnsubscribeId<ActionPressedSignal>(_id, OnActionPressed);
                    break;
                case Type.Cancel:
                    _signalBus.TryUnsubscribeId<CancelPressedSignal>(_id, OnActionPressed);
                    break;
            }
        }

        private void OnActionPressed()
        {
            TriggerAction.Trigger(onPressed);
        }
    }
}
