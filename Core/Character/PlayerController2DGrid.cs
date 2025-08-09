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
        public LayerMask layerMask;

        [Inject] private SignalBus signalBus;
        [Inject(Id = "PlayerId")] private string _id;

        private Vector2 movement;

        public enum State
        {
            Idle,
            Moving
        }
        public State CurrentState { get; private set; }

        private NextMovement target;

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
            {
                TriggerAction.Trigger(OnMoved);
            }
        }

        private float time;

        private void FixedUpdate()
        {
            switch (CurrentState)
            {
                case State.Idle:
                    if (movement.magnitude > 0.1f)
                    {
                        Vector2 targetPos = GetNextGridCenter(rigidBody.position, movement);
                        target = new NextMovement()
                        {
                            startPos = rigidBody.position,
                            endPos = targetPos,
                            startTime = 1f - Vector2.Distance(rigidBody.position, targetPos)
                        };
                        time = target.startTime;
                        CurrentState = State.Moving;
                    }
                    break;

                case State.Moving:
                    time += Time.fixedDeltaTime * speed;

                    float t = Mathf.Clamp01(time);
                    Vector2 newPos = Vector2.Lerp(target.startPos, target.endPos, t);
                    rigidBody.MovePosition(newPos);

                    if (time >= 1f)
                    {
                        // Snap to exact target position
                        rigidBody.MovePosition(target.endPos);

                        if (movement.magnitude > 0.1f)
                        {
                            Vector2 nextTargetPos = GetNextGridCenter(target.endPos, movement);
                            target = new NextMovement()
                            {
                                startPos = target.endPos,
                                endPos = nextTargetPos,
                                startTime = 1f - Vector2.Distance(rigidBody.position, nextTargetPos)
                            };
                            time = target.startTime;
                        }
                        else
                        {
                            CurrentState = State.Idle;
                        }
                    }
                    break;
            }
        }



        /*private void FixedUpdate()
        {
            var direction = nextPosition - rigidBody.position;
            var position = Vector3.MoveTowards(rigidBody.position, nextPosition, speed * Time.fixedDeltaTime);
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
            {
                position = new Vector3(Mathf.MoveTowards(position.x, nextPosition.x, speed * Time.fixedDeltaTime), position.y, position.z);
            } else if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                position = new Vector3(position.x, Mathf.MoveTowards(position.y, nextPosition.y, speed * Time.fixedDeltaTime), position.z);
            }

            rigidBody.MovePosition(position);
        }*/

        private struct NextMovement
        {
            public Vector2 startPos;
            public Vector2 endPos;
            public float startTime;
        }

        private Vector2 GetNextGridCenter(Vector2 position, Vector2 direction)
        {
            direction = direction.normalized;

            Vector2 current = SnapToGridCenter(position);

            Vector2 step = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ?
                (direction.x > 0.6f ? new Vector2(1f, 0f)
                : direction.x < -0.6f ? new Vector2(-1f, 0f) : new Vector2())
                : direction.y > 0.6f ? new Vector2(0f, 1f)
                : direction.y < -0.6f ? new Vector2(0f, -1f) : new Vector2();

            var goal = current + step;
            var filter = new ContactFilter2D()
            {
                useTriggers = false,
                layerMask = layerMask,
                useLayerMask = true,
            };
            var array = new Collider2D[1];
            var collider = Physics2D.OverlapPoint(goal, filter, array);
            if (collider > 0)
            {
                Debug.LogWarning(array[0].name);
                return current;
            }

            return goal;
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
