using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Kiskovi.Core
{
    public abstract class Placement : MonoBehaviour, IDropHandler
    {
        private Stack<Dragable> dragables = new Stack<Dragable>();
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var dragable = eventData.pointerDrag.GetComponent<Dragable>();

                if (dragable == null || !IsAccaptable(dragable)) return;

                dragable.Dropped(this, GetOffset());

                dragables.Push(dragable);

                OnDropped(dragable);
            }
        }

        protected void ReleaseAll()
        {
            while (dragables.Count > 0)
            {
                var dragable = dragables.Pop();
                dragable.Release();
            }
        }

        public abstract void OnDropped(Dragable dragable);

        public virtual Vector3 GetOffset()
        {
            return Vector3.zero;
        }

        public virtual bool IsAccaptable(Dragable dragable)
        {
            return dragable.IsDragable;
        }
    }
}