using System.Collections;
using UnityEngine;

namespace Kiskovi.Core
{
    internal class OnDelayTrigger : MonoBehaviour
    {
        public float delay;
        public GameAction action;

        private void Start()
        {
            StartCoroutine(DelayTrigger());
        }

        private IEnumerator DelayTrigger()
        {
            yield return new WaitForSeconds(delay);
            GameAction.Trigger(action);
        }
    }
}
