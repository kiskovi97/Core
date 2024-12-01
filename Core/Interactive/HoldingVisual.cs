using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kiskovi.Core
{
    internal class HoldingVisual : MonoBehaviour
    {
        public Image valueImage;
        public TriggerAction OnFilledUp;

        private bool isHolding;

        [Inject] private SignalBus _signalBus;

        private void OnEnable()
        {
            _signalBus.Subscribe<ActionHoldSignal>(OnHold);
            isHolding = false;
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<ActionHoldSignal>(OnHold);
        }

        private void OnHold(ActionHoldSignal signal)
        {
            isHolding = signal.isHolding;
        }

        private void Update()
        {
            if (valueImage == null) return;

            if (isHolding) {
                valueImage.fillAmount += Time.deltaTime;
                if (valueImage.fillAmount >= 1f)
                {
                    valueImage.fillAmount = 0f;
                    TriggerAction.Trigger(OnFilledUp);
                }
            } else
            {
                valueImage.fillAmount = 0;
            }
        }
    }
}
