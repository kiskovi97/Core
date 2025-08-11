using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    internal class SpawnObjectAction : TriggerAction
    {
        public Transform target;
        public GameObject prefab;

        [Inject] private DiContainer container;

        public override void Trigger(params object[] parameter)
        {
            base.Trigger(parameter);

            if (container != null)
            {
                container.InstantiatePrefab(prefab, target.position, target.rotation, null);
            } else
            {
                Instantiate(prefab, target.position, target.rotation);
            }
        }
    }
}
