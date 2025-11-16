using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    [RequireComponent(typeof(Selectable))]
    public class SelectableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        protected Selectable selectable;

        [SerializeField] protected GameObject hoverObject;
        [SerializeField] protected GameObject onlyHoverObject;
        [SerializeField] protected GameObject notHoverObject;

        [SerializeField] protected bool isHoverEnabled;
        [SerializeField] protected bool isResizeingEnabled = false;
        [SerializeField] protected bool isInteractableCounted = true;
        [SerializeField] protected bool isControllerAlwaysVisible = false;
        [SerializeField] protected bool isToggleCounted = true;
        [SerializeField] protected float resizePercantage = 1.3f;

        private Canvas canvasParent;
        private RectTransform myRectTransform;
        private Vector2 originalSize;
        private Vector2? originalSizeOverride;
        private Vector2 OriginalSize => originalSizeOverride.HasValue ? originalSizeOverride.Value : originalSize;

        [SerializeField] public UnityEvent<bool> OnHoverEvent;
        [SerializeField] public UnityEvent<bool> OnSelectedEvent;
        private bool isHovering = false;

        public void OverrideSize(Vector2 original)
        {
            originalSizeOverride = original;
        }

        protected virtual void Start()
        {
            myRectTransform = GetComponent<RectTransform>();
            if (myRectTransform != null)
                originalSize = myRectTransform.localScale;
            GetInitComponents();
            OnNotHover();
        }

        protected virtual void OnEnable()
        {
            if (myRectTransform == null) return;
            isHovering = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
        }

        private void GetInitComponents()
        {
            if (selectable == null)
                selectable = GetComponent<Selectable>();
            if (canvasParent == null)
                canvasParent = GetComponentInParent<Canvas>();
        }

        public void Init(Sprite image)
        {
            GetInitComponents();
            if (image != null)
                transform.Find("image").GetComponent<Image>().sprite = image;
        }

        protected virtual void Update()
        {
            if (isHoverEnabled &&
                (isHovering
                || (selectable is Toggle toggle && toggle.isOn && isToggleCounted)
                || (EventSystem.current.currentSelectedGameObject == selectable.gameObject)
                || isControllerAlwaysVisible)
                && (!isInteractableCounted || selectable.interactable))
                OnHover();
            else
                OnNotHover();
        }

        private bool lastHovered = false;
        private bool lastSelected = false;

        private void OnNotHover()
        {
            if (hoverObject != null)
                hoverObject.SetActive(false);
            if (onlyHoverObject != null)
                onlyHoverObject.SetActive(false);
            if (notHoverObject != null)
                notHoverObject.SetActive(true);
            if (lastHovered)
            {
                lastHovered = false;
                OnHoverEvent?.Invoke(false);
            }
            if (lastSelected)
            {
                lastSelected = false;
                OnSelectedEvent?.Invoke(false);
            }

            if (myRectTransform != null && isResizeingEnabled)
            {
                myRectTransform.localScale = OriginalSize;
            }
        }

        protected virtual void OnHover()
        {
            if (hoverObject != null)
                hoverObject.SetActive(isHoverEnabled);
            if (onlyHoverObject != null)
                onlyHoverObject.SetActive(isHoverEnabled && (isHovering || isControllerAlwaysVisible)); // && InputSignals.Scheme != ControlScheme.Keyboard
            if (notHoverObject != null)
                notHoverObject.SetActive(!isHoverEnabled);
            if (onlyHoverObject != null)
            {
                if (lastHovered != onlyHoverObject.activeSelf)
                {
                    lastHovered = onlyHoverObject.activeSelf;
                    OnHoverEvent?.Invoke(onlyHoverObject.activeSelf);
                }
            }
            else
            {
                if (!lastHovered)
                {
                    lastHovered = true;
                    OnHoverEvent?.Invoke(true);
                }
            }
            if (!lastSelected)
            {
                lastSelected = true;
                OnSelectedEvent?.Invoke(true);
            }

            if (myRectTransform != null && isResizeingEnabled)
            {
                myRectTransform.localScale = OriginalSize * resizePercantage;
            }
        }
    }
}
