using UnityEngine;

namespace Kiskovi.Core
{
    internal class OnEnableTrigger : MonoBehaviour
    {
        public TriggerAction action;
        public bool onlyReEnable;
        public bool onlyWhenTimeIsOn;

        private bool disabledOnce;

        private bool triggered;

        private void OnEnable()
        {
            if (!onlyReEnable || disabledOnce)
                if (!onlyWhenTimeIsOn || Time.timeScale > 0.1f)
                    TriggerAction.Trigger(action);
        }

        private void Update()
        {
            if (!triggered && onlyWhenTimeIsOn && Time.timeScale > 0.1f)
            {
                triggered = true;
                TriggerAction.Trigger(action);
            }
        }

        private void OnDisable()
        {
            triggered = false;
            disabledOnce = true;
        }
    }
}
