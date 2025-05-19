using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{

    internal class SignalTrigger<T> : MonoBehaviour where T : class
    {

        [Inject] private SignalBus _signalBus;
        [Inject(Id = "PlayerId")] private string _id;

        public TriggerAction onPressed;

        private void OnEnable()
        {
            _signalBus.SubscribeId<T>(_id, OnActionPressed);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribeId<T>(_id, OnActionPressed);
        }

        private void OnActionPressed()
        {
            TriggerAction.Trigger(onPressed);
        }
    }
}
