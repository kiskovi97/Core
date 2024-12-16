using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    internal class PlayerController : PlayerControllerBase
    {
        public Rigidbody rigidBody;
        public float speed = 1.0f;

        [Inject] private SignalBus signalBus;

        private Vector2 movement;
        public override Vector2 Movement => movement;

        private void OnEnable()
        {
            signalBus.Subscribe<MoveSignal>(OnMove);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<MoveSignal>(OnMove);
        }

        private void OnMove(MoveSignal signal)
        {
            movement = signal.Move;
        }
        private void FixedUpdate()
        {
            if (Time.timeScale < 0.01) return;
            var move = new Vector3(movement.x, 0f, movement.y);
            rigidBody.MovePosition(rigidBody.position + move * speed * Time.fixedDeltaTime);
            //rigidBody.AddForce(move * speed, ForceMode.Force);
        }
    }
}
