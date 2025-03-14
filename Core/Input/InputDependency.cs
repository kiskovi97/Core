using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    internal class InputDependency : MonoBehaviour
    {
        public InputInfoGroup inputInfo;

        [Inject] private IAvailableInputManager manager;

        private void OnEnable()
        {
            manager.RegisterInput(inputInfo, this);
        }

        private void OnDisable()
        {
            manager.DeRegisterInput(inputInfo, this);
        }
    }
}
