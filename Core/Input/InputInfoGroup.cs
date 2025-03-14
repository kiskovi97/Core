using UnityEngine.Localization;
using UnityEngine;
using System;

namespace Kiskovi.Core
{
    [CreateAssetMenu(fileName = "Input", menuName = "KiskoviCore/InputInfo")]
    public class InputInfoGroup : ScriptableObject
    {
        public InputInfo[] inputInfos;
    }

    [Serializable]
    public class InputInfo : IData
    {
        public ControlScheme controlScheme;
        public LocalizedString referenceName;
        public Sprite icon;
    }
}
