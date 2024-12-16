using UnityEngine;

namespace Kiskovi.Core
{
    internal class PlayerControllerAnimation : MonoBehaviour
    {
        public PlayerControllerBase playerController;
        public Animator animator;

        public string movingAxiyXKey = "MovingX";
        public string movingAxiyYKey = "MovingY";

        private void Update()
        {
            if (animator != null)
            {
                animator.SetFloat(movingAxiyXKey, playerController.Movement.x);
                animator.SetFloat(movingAxiyYKey, playerController.Movement.y);
            }
        }
    }
}
