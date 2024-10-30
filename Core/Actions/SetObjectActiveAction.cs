using UnityEngine;

namespace Kiskovi.Core
{
    internal class SetObjectActiveAction : TriggerAction
    {
        public GameObject[] objects;

        public bool isActive;

        public override void Trigger(params object[] parameter)
        {
            foreach(var obj in objects)
            {
                obj.SetActive(isActive);
            }
        }
    }
}
