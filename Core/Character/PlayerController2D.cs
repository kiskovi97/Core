using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    public class PlayerController2D : PlayerControllerBase
    {
        public static PlayerController2D Instance;

        public Rigidbody2D rigidBody;
        public float speed = 1.0f;

        [Inject] private SignalBus signalBus;

        private Vector2 movement;

        public override Vector2 Movement => movement;

        private void OnEnable()
        {
            signalBus.Subscribe<MoveSignal>(OnMove);

            Instance = this;
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
            var move = new Vector2(movement.x, movement.y);
            rigidBody.AddForce(move * speed, ForceMode2D.Force);
            //rigidBody.AddForce(move * speed, ForceMode.Force);
        }
    }
}
