using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

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
        public bool resetPositionOnRelease = true;
        public float alphaOnDrag = 0.9f;
        public float dragSpeed = 300f;
        protected Placement placement;

        private Vector3 startPosition;
        private Vector2 direction;

        public virtual bool IsDragable => true;

        [Inject] private SignalBus _signalBus;
        [Inject] private ISelectionSystem selectionSystem;

        private void OnEnable()
        {
            _signalBus.Subscribe<UIInteractions.DragUI>(OnDragUI);
        }

        private void OnDisable()
        {

            _signalBus.Unsubscribe<UIInteractions.DragUI>(OnDragUI);
        }

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
                if (resetPositionOnRelease)
                {
                    rectTransform.localPosition = startPosition;
                }
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

        private void OnDragUI(UIInteractions.DragUI signal)
        {
            direction = signal.value;
        }

        private void Update()
        {
            if (selectionSystem.CurrentSelectedObj != gameObject) return;

            if (direction != Vector2.zero)
            {
                rectTransform.anchoredPosition += direction * dragSpeed * Time.unscaledDeltaTime;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            selectionSystem.SetSelected(gameObject);
        }
    }
}