using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class PlayerController2DGrid : PlayerControllerBase
    {
        public static PlayerController2DGrid Instance;

        public Rigidbody2D rigidBody;
        public float speed = 1.0f;
        public TriggerAction OnMoved;

        [Inject] private SignalBus signalBus;
        [Inject(Id = "PlayerId")] private string _id;

        private Vector2 movement;
        private Vector2 nextPosition;

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
            movement = signal.value;
            if (movement.magnitude > 0f)
                TriggerAction.Trigger(OnMoved);
        }

        private void Update()
        {
            nextPosition = GetNextGridCenter(transform.position, movement);
        }

        private void FixedUpdate()
        {
            var direction = nextPosition - rigidBody.position;

            if (direction.magnitude > 0.01f)
            {
                var move = direction.normalized * Mathf.Min(1f, direction.magnitude);

                rigidBody.AddForce(move * speed, ForceMode2D.Force);
            } else
            {
                rigidBody.MovePosition(nextPosition);
            }
        }

        private static Vector2 GetNextGridCenter(Vector2 position, Vector2 direction)
        {
            if (direction == Vector2.zero)
                return SnapToGridCenter(position);

            direction = direction.normalized;

            Vector2 current = SnapToGridCenter(position);

            Vector2 step = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ?
                (direction.x > 0.6f ? new Vector2(1f, 0f) 
                : direction.x < -0.6f ? new Vector2(-1f, 0f) : new Vector2())
                : direction.y > 0.6f ? new Vector2(0f, 1f)
                : direction.y < -0.6f ? new Vector2(0f, -1f) : new Vector2();

            return current + step;
        }

        private static Vector2 SnapToGridCenter(Vector2 position)
        {
            return new Vector2(
                Mathf.Floor(position.x) + 0.5f,
                Mathf.Floor(position.y) + 0.5f
            );
        }
    }
}
