using UnityEngine;

namespace Kiskovi.Core
{
    internal class OnEnableTrigger : MonoBehaviour
    {
        public GameAction gameAction;
        public bool onlyReEnable;
        public bool onlyWhenTimeIsOn;

        private bool disabledOnce;

        private bool triggered;

        private void OnEnable()
        {
            if (!onlyReEnable || disabledOnce)
                if (!onlyWhenTimeIsOn || Time.timeScale > 0.1f)
                    GameAction.Trigger(gameAction);
        }

        private void Update()
        {
            if (!triggered && onlyWhenTimeIsOn && Time.timeScale > 0.1f)
            {
                triggered = true;
                GameAction.Trigger(gameAction);
            }
        }

        private void OnDisable()
        {
            triggered = false;
            disabledOnce = true;
        }
    }
}
