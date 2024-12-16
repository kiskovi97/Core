using UnityEngine;

namespace Kiskovi.Core
{
    public abstract class PlayerControllerBase : MonoBehaviour
    {
        public abstract Vector2 Movement { get; }
    }
}
