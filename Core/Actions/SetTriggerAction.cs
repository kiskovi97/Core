using UnityEngine;

namespace Kiskovi.Core
{
    public class SetTriggerAction : TriggerAction
    {
        public string trigger;
        public Animator animator;

        public override void Trigger(params object[] parameter)
        {
            if (animator != null)
                animator.SetTrigger(trigger);
        }
    }
}