using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Kiskovi.Core
{
    internal class InputSystemTriggers : MonoBehaviour
    {
        public TriggerAction onAnyButtonpressed;

        private void OnEnable()
        {
            InputSystem.onAnyButtonPress.CallOnce(ctrl => TriggerAction.Trigger(onAnyButtonpressed));
        }
    }
}
