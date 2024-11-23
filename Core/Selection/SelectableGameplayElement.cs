using System;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Kiskovi.Core
{
    [RequireComponent(typeof(Collider2D))]
    internal class SelectableGameplayElement : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        [SerializeField] protected GameObject glowSprite;
        [SerializeField] private GameObject hover;
        [SerializeField] private int layer;
        [SerializeField] private TriggerAction onMainAction;
        protected bool isHover { get; private set; }
        protected bool isSelected { get; private set; }

        protected bool outerAccessible = true;

        protected virtual bool InnerAccessible => true;

        public bool Accesable
        {
            get => InnerAccessible && outerAccessible;
            set => outerAccessible = value;
        }

        public virtual int Priority => 0;

        protected Collider2D myCollider { get; private set; }

        public event Action onSelected;
        public event Action onPointerDown;

        protected virtual void Awake()
        {
            myCollider = GetComponent<Collider2D>();
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
            isHover = false;
            isSelected = false;
            Update();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            PerformedMainAction();
            onPointerDown?.Invoke();
        }

        protected virtual void Update()
        {
            if (glowSprite != null)
                glowSprite.SetActive(Accesable && (isHover || isSelected));
            if (hover != null)
                hover.SetActive(Accesable && isHover);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //PerformedMainAction();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isSelected = true;
            eventData.selectedObject = gameObject;
            PerformedMainAction();
            onPointerDown?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHover = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHover = false;
        }

        protected virtual void Select(BaseEventData eventData)
        {
            if (!Accesable) return;
            isSelected = true;
            onSelected?.Invoke();
        }

        protected virtual void DeSelect(BaseEventData eventData)
        {
            isSelected = false;
        }

        protected virtual void PerformedMainAction()
        {
            if (Accesable)
            {
                TriggerAction.Trigger(onMainAction);
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            Select(eventData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            DeSelect(eventData);
        }
    }
}
