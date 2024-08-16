using UnityEngine;

namespace Kiskovi.Core
{
    internal class GoToLastChild : MonoBehaviour
    {
        private void Update()
        {
            if (transform.GetSiblingIndex() < transform.parent.childCount)
            {
                transform.SetAsLastSibling();
            }
        }
    }
}
