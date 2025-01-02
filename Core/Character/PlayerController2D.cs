using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    public class PlayerController2D : PlayerControllerBase
    {
        public static PlayerController2D Instance;

        public Rigidbody2D rigidBody;
        public float speed = 1.0f;
        public bool isMultiFriendly;

        [Inject] private SignalBus signalBus;
        [Inject(Id = "PlayerId")] private string _id;

        private Vector2 movement;

        public override Vector2 Movement => movement.normalized;

        private void OnEnable()
        {
            signalBus.SubscribeId<MoveSignal>(_id, OnMove);

            Instance = this;
        }

        private void OnDisable()
        {
            signalBus.TryUnsubscribeId<MoveSignal>(_id, OnMove);
            movement = Vector2.zero;
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
