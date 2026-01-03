using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    public static class MouseUitilities
    {
        public static Vector2 GetMousePosition(RectTransform rectTransform)
        {
            Vector2 mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                Mouse.current.position.value,
                null,
                out mousePosition
            );
            return mousePosition;
        }
    }
}
