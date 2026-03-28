using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    public static class InputUtils
    {
        public static Vector2 CursorPos => Mouse.current.position.ReadValue();
    }
}
