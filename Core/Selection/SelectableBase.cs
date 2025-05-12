using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class SelectableBase : MonoBehaviour
    {
        public UIPanel parent;

        [Inject] protected GlobalSelectionSystem selectionSystem;
        private bool _prevCanBeSelected;

        public virtual bool CanBeSelected => isActiveAndEnabled && (parent == null || parent.isInFront);
        public virtual int Priority => 0;

        protected virtual void Awake()
        {
            if (parent == null)
                parent = GetComponentInParent<UIPanel>(true);
            selectionSystem.Register(parent, this);
        }

        protected virtual void OnEnable()
        {
            selectionSystem.UpdateByInstance(this);
        }
        protected virtual void Update()
        {
            if (_prevCanBeSelected != CanBeSelected)
            {
                _prevCanBeSelected = CanBeSelected;
                selectionSystem.UpdateByInstance(this);
            }
        }

        protected virtual void OnDisable()
        {
            selectionSystem.UpdateByInstance(this);
        }

        protected virtual void OnDestroy()
        {
            selectionSystem.DeRegister(parent, this);
        }
    }
}
