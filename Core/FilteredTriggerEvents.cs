using UnityEngine;

namespace Kiskovi.Core
{
    internal class FilteredTriggerEvents : MonoBehaviour
    {
        public bool onlyNonTrigger;
        public string filterTag;

        public TriggerAction onTriggerEnter;
        public TriggerAction onTriggerExit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((string.IsNullOrEmpty(filterTag) || collision.gameObject.CompareTag(filterTag))
                && (!onlyNonTrigger || !collision.isTrigger))
            {
                TriggerAction.Trigger(onTriggerEnter);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if ((string.IsNullOrEmpty(filterTag) || collision.gameObject.CompareTag(filterTag))
                && (!onlyNonTrigger || !collision.isTrigger))
            {
                TriggerAction.Trigger(onTriggerExit);
            }
        }
    }
}
