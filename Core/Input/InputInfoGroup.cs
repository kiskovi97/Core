using UnityEngine.Localization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    [CreateAssetMenu(fileName = "Input", menuName = "KiskoviCore/InputInfo")]
    public class InputInfoGroup : ScriptableObject, IData
    {
        public LocalizedString referenceName;
        public InputActionReference inputActionReference;
    }
}
