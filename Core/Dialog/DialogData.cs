using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;

namespace Kiskovi.Core
{
    public enum DialogSpeaker
    {
        MainCharacter,
        NPC,
    }
    [System.Serializable]
    public struct DialogLine
    {
        public DialogSpeaker speaker;
        public LocalizedString text;
    }

    [CreateAssetMenu(fileName = "DialogData", menuName = "KiskoviCore/DialogData", order = 1)]
    public class DialogData : ScriptableObject, IData
    {
        public List<DialogLine> Lines = new List<DialogLine>();
    }
}
