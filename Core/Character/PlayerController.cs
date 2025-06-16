using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    internal class PlayerController : PlayerControllerBase
    {
        public Rigidbody rigidBody;
        public float speed = 1.0f;

        [Inject] private SignalBus signalBus;
        [Inject(Id = "PlayerId")] private string _id;

        private Vector2 movement;
        public override Vector2 Movement => movement.normalized;

        private void OnEnable()
        {
            signalBus.SubscribeId<MoveSignal>(_id, OnMove);
        }

        private void OnDisable()
        {
            signalBus.TryUnsubscribeId<MoveSignal>(_id, OnMove);
            movement = Vector2.zero;
        }

        private void OnMove(MoveSignal signal)
        {
            movement = signal.value;
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
