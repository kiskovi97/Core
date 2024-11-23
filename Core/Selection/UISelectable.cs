using UnityEngine.UI;
using UnityEngine;

namespace Kiskovi.Core
{
    [RequireComponent(typeof(Selectable))]
    internal class UISelectable : SelectableBase
    {
        public Selectable selectable;
        public int priority = 0;

        public override int Priority => base.Priority + priority;
        public override bool CanBeSelected => base.CanBeSelected && selectable != null && selectable.interactable;

        protected override void Awake()
        {
            base.Awake();
            selectable = GetComponent<Selectable>();
        }
    }
}
