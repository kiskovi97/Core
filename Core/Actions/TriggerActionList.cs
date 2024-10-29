using UnityEngine;

namespace Kiskovi.Core
{
    internal class TriggerActionList : TriggerAction
    {
        public TriggerAction[] Actions;
        public bool triggerActionsOfChildren = false;

        public override void Trigger(params object[] parameter)
        {
            foreach (var action in Actions)
            {
                Trigger(action);
            }

            if (triggerActionsOfChildren)
            {
                foreach (Transform trans in transform)
                {
                    Trigger(trans.GetComponent<TriggerAction>());
                }
            }
        }
    }
}
