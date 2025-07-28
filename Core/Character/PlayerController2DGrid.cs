using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class PlayerController2DGrid : PlayerControllerBase
    {
        public static PlayerController2DGrid Instance;

        public Rigidbody2D rigidBody;
        public float speed = 1.0f;
        public float smoothTime = 0.08f;
        public TriggerAction OnMoved;

        [Inject] private SignalBus signalBus;
        [Inject(Id = "PlayerId")] private string _id;

        private Vector2 movement;
        private Vector2 nextPosition;
        private Vector2 velocity = Vector2.zero;

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


            //rigidBody.MovePosition(Vector2.MoveTowards(rigidBody.position, nextPosition, speed * Time.deltaTime));
        }

        private void FixedUpdate()
        {
            Vector2 newPos = Vector2.MoveTowards(
                transform.position,
                nextPosition,
                Time.fixedDeltaTime * speed
            );

            transform.position = newPos;
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
