using UnityEngine;

namespace Kiskovi.Core
{
    internal class ResetRebindingUI : MonoBehaviour
    {
        public RebindingUI[] rebindingUIs;

        public void ResetAll()
        {
            foreach (var rebinding in rebindingUIs)
                rebinding.ResetToDefault();
        }
    }
}
