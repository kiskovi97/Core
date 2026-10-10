using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class ResetRebindingUI : MonoBehaviour
    {
        public RebindingUI[] rebindingUIs;

        [Inject]
        private IRebindSaveLoad _rebindSaveLoad;

        public void ResetAll()
        {
            foreach (var rebinding in rebindingUIs)
                rebinding.ResetToDefault();
            _rebindSaveLoad.ResetAllBindings();
        }
    }
}
