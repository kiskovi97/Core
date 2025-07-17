using UnityEngine;

namespace Kiskovi.Core
{
    internal class PlayerController2DCenterOffset : MonoBehaviour
    {
        public PlayerController2D playerController2D;
        public float offset = 0.2f;

        private void Update()
        {
            var movement = playerController2D.Movement;
            if (movement.sqrMagnitude > 0.1f)
            {
                transform.localPosition = movement.normalized * offset;
            }
        }
    }
}
