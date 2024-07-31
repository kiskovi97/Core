using UnityEngine;

namespace Kiskovi.Core
{
    public class TriggerAction : MonoBehaviour
    {
        public void TriggerThis()
        {
            Trigger();
        }

        public static void Trigger(TriggerAction action, params object[] parameter)
        {
            if (action != null)
            {
                action.Trigger(parameter);
            }
        }

        public virtual void Trigger(params object[] parameter)
        {

        }
    }
}
