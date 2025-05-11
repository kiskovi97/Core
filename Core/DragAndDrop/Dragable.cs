using UnityEngine;
using UnityEngine.EventSystems;

namespace Kiskovi.Core
{
    public class Dragable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public RectTransform rectTransform;
        public RectTransform rectTransformParent;
        public RectTransform rectTransformDragParent;
        public Transform originalParent;
        public CanvasGroup canvasGroup;
        public bool removeParentWhenDrag;
        public float alphaOnDrag = 0.9f;
        protected Placement placement;
        private Vector3 startPosition;

        public virtual bool IsDragable => true;

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!IsDragable) return;
            placement = null;
            canvasGroup.alpha = alphaOnDrag;
            canvasGroup.blocksRaycasts = false;
            startPosition = rectTransform.localPosition;
            rectTransform.localScale = Vector3.one;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.SetParent(rectTransformDragParent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!IsDragable) return;
            rectTransform.position = eventData.position;
            rectTransform.localScale = Vector3.MoveTowards(rectTransform.localScale, Vector3.one * 1.2f, Time.deltaTime);
            rectTransform.rotation = Quaternion.RotateTowards(rectTransform.rotation, Quaternion.identity, Time.deltaTime * 30f);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (placement == null)
            {
                rectTransform.SetParent(rectTransformParent);
                rectTransform.localPosition = startPosition;
            }
            rectTransform.localScale = Vector3.one;
            rectTransform.localRotation = Quaternion.identity;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        public void Dropped(Placement placement, Vector3 offset)
        {
            this.placement = placement;
            transform.SetParent(placement.target, true);
            transform.SetPositionAndRotation(placement.target.position + offset, placement.target.rotation);
        }

        public void Release()
        {
            placement = null;
            rectTransform.SetParent(rectTransformParent, false);
            rectTransform.localPosition = startPosition;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }
    }
}