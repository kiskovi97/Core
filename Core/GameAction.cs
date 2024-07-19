using UnityEngine;

namespace Kiskovi.Core
{
    public class GameAction : MonoBehaviour
    {
        public void TriggerThis()
        {
            Trigger();
        }

        public static void Trigger(GameAction action, params object[] parameter)
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
