using UnityEngine;

namespace Kiskovi.Core
{
    internal class EnableGameObject : TriggerAction
    {
        public GameObject obj;
        public bool enable;

        public override void Trigger(params object[] parameter)
        {
            obj.SetActive(enable);
        }
    }
}
