using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class PopupObject : MonoBehaviour
    {
        public AnimatedObject infoObj;
        public TriggerAction OnInteract;

        [Inject] private SignalBus signalBus;

        protected void OnEnable()
        {
            if (infoObj != null)
            {
                infoObj.SetActive(false);
            }
            signalBus.Subscribe<UIInteractions.AcceptSignal>(OnInterract);
        }

        protected void OnDisable()
        {
            if (infoObj != null)
            {
                infoObj.SetActive(false);
            }
            signalBus.TryUnsubscribe<UIInteractions.AcceptSignal>(OnInterract);
        }

        protected virtual void OnInterract()
        {
            TriggerAction.Trigger(OnInteract);

            if (infoObj != null)
            {
                infoObj.SetActive(!infoObj.PrevValue);
            }
        }
    }
}
