using Kiskovi.Core;

using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

namespace PuzzleProject.Core
{
    internal class PlayerSignalSenderBehaviour : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject(Id = "PlayerId")] private string _id;

        public void OnMove(InputValue context)
        {
            _signalBus.TryFireId(_id, new MoveSignal(context.Get<Vector2>()));
        }
    }
}
