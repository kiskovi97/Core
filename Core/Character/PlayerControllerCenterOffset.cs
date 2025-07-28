using UnityEngine;

namespace Kiskovi.Core
{
    internal class PlayerControllerCenterOffset : MonoBehaviour
    {
        public PlayerControllerBase playerController2D;
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
