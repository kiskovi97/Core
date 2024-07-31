using System.Collections;
using UnityEngine;

namespace Kiskovi.Core
{
    internal class OnDelayTrigger : MonoBehaviour
    {
        public float delay;
        public TriggerAction action;

        private void Start()
        {
            StartCoroutine(DelayTrigger());
        }

        private IEnumerator DelayTrigger()
        {
            yield return new WaitForSeconds(delay);
            TriggerAction.Trigger(action);
        }
    }
}
