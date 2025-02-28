using UnityEngine;

namespace Kiskovi.Core
{
    internal class TriggerAnimationAction : TriggerAction
    {
        public Animator animator;
        public string triggerName;
        public override void Trigger(params object[] parameter)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
