using Kiskovi.Core;
using UnityEngine;
using Zenject;

namespace PuzzleProject.Dialog
{
    public class StartDialogTriggerAction : TriggerAction
    {
        public DialogData dialogData;

        [Inject]
        private IDialogSystem system;

        public override void Trigger(params object[] parameter)
        {
            base.Trigger(parameter);
            system.StartDialog(dialogData);
        }
    }
}
