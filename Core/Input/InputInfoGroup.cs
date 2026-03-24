using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

namespace Kiskovi.Core
{
    [CreateAssetMenu(fileName = "Input", menuName = "KiskoviCore/InputInfo")]
    public class InputInfoGroup : ScriptableObject, IData
    {
        public LocalizedString title;
        public InputActionReference inputActionReference;
    }
}
