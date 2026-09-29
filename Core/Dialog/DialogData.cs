using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Kiskovi.Core
{
    public enum DialogSpeaker
    {
        MainCharacter,
        NPC_Alex,
        NPC_Bob,
        NPC_Detective,
        Narrator,
        NPC_Marta,
        NPC_Klara,
        NPC_Boss,
    }

    public enum EndDialogEvent
    {
        None,
        EndGame,
        StartMenu,
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
        public EndDialogEvent endDialogEvent = EndDialogEvent.None;
        public DialogSpeaker mainSpeaker = DialogSpeaker.NPC_Alex;
    }
}
