using UnityEngine;

namespace Kiskovi.Core
{
    internal class EnableGameObject : GameAction
    {
        public GameObject obj;
        public bool enable;

        public override void Trigger(params object[] parameter)
        {
            obj.SetActive(enable);
        }
    }
}
