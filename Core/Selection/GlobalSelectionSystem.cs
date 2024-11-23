using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine.EventSystems;
using Zenject;

namespace Kiskovi.Core
{
    public class SelectionClearSignal { }

    internal class GlobalSelectionSystem : ITickable, IInitializable, IDisposable
    {
        private Dictionary<UIPanel, List<SelectableBase>> selectables = new Dictionary<UIPanel, List<SelectableBase>>();
        private Dictionary<UIPanel, SelectableBase> cache = new Dictionary<UIPanel, SelectableBase>();
        private List<SelectableBase> worldSelectables = new List<SelectableBase>();

        private SignalBus _signalBus;

        private EventSystem eventSystem => EventSystem.current;
        public SelectableBase CurrentSelected { get; private set; }
        public bool CanNavigate => !eventSystem.alreadySelecting;
        public event Action OnChangedEvent;

        private bool onChangedRequest = false;

        public GlobalSelectionSystem(SignalBus signalBus)
        {
            _signalBus = signalBus;
            UIPanel.onChanged += OnChanged;

            OnChanged();
        }

        public void Initialize()
        {
            _signalBus.Subscribe<InputSignals.ControlSchemeChanged>(OnControlSchemeChanged);
            _signalBus.Subscribe<SelectionClearSignal>(OnClear);
            OnChanged();
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<InputSignals.ControlSchemeChanged>(OnControlSchemeChanged);
            _signalBus.TryUnsubscribe<SelectionClearSignal>(OnClear);
        }

        private void OnClear()
        {
            SetSelectedGameObject(null);
        }

        private void OnControlSchemeChanged(InputSignals.ControlSchemeChanged signal)
        {
            SetNewControlSchem(signal.Scheme);
        }

        private void OnChanged()
        {
            onChangedRequest = true;
        }

        private void SetNewControlSchem(ControlScheme scheme)
        {
            switch (scheme)
            {
                case ControlScheme.Keyboard:
                    SetKeyboardWorkflow();
                    break;
                default:
                    SetControllerWorkflow();
                    break;
            }
            OnChangedEvent?.Invoke();
        }

        private void SetKeyboardWorkflow()
        {
        }

        private void SetControllerWorkflow()
        {
            var list = GetCurrentLayerSelectables();
            var highestPriority = list.Any() ? list.Max(item => item.Priority) : 0;

            if (CurrentSelected != null && CurrentSelected.CanBeSelected && list.Contains(CurrentSelected) && CurrentSelected.Priority >= highestPriority)
                return;

            var first = list.FirstOrDefault();
            if (first != null && first.parent != null && cache.TryGetValue(first.parent, out var cached) && cached != null && cached.CanBeSelected && cached.Priority >= first.Priority)
            {
                SetSelectedGameObject(cached);
            }
            else
            {
                SetSelectedGameObject(first);
            }
        }

        public void SetSelectedGameObject(SelectableBase nextSelected)
        {
            if (nextSelected != null && nextSelected.parent != null)
            {
                if (cache.ContainsKey(nextSelected.parent))
                {
                    cache[nextSelected.parent] = nextSelected;
                }
                else
                {
                    cache.Add(nextSelected.parent, nextSelected);
                }
            }
            CurrentSelected = nextSelected;
        }

        internal void Deselect(AutomaticSelection automaticSelection)
        {
            if (CurrentSelected == automaticSelection)
                SetSelectedGameObject(null);
        }

        public IEnumerable<SelectableBase> GetCurrentLayerSelectables(bool includeNonUI = true)
        {
            var list = new List<SelectableBase>();

            foreach (var panel in selectables.Keys)
            {
                if (panel.isInFront && panel.isOpened && (!panel.isUINavigationBlocked || includeNonUI))
                {
                    return selectables[panel].Where(item => item.CanBeSelected).OrderByDescending(item => item.Priority);
                }
            }

            if (UIWindow.IsWindowOpen || !includeNonUI) return new List<SelectableBase>();

            return worldSelectables.Where(item => item.CanBeSelected).OrderByDescending(item => item.Priority);
        }

        public void Register(UIPanel panel, SelectableBase selectable)
        {
            if (panel == null)
            {
                worldSelectables.Add(selectable);
                OnChanged();
                return;
            }

            if (!selectables.ContainsKey(panel))
            {
                selectables.Add(panel, new List<SelectableBase>());
            }

            selectables[panel].Add(selectable);
            OnChanged();
        }

        public void DeRegister(UIPanel panel, SelectableBase selectable)
        {
            if (panel == null)
            {
                worldSelectables.Remove(selectable);
                OnChanged();
                return;
            }
            if (selectables.ContainsKey(panel))
            {
                selectables[panel].Remove(selectable);
                if (selectables[panel].Count == 0)
                    selectables.Remove(panel);
            }
            OnChanged();
        }

        public void Tick()
        {
            if (InputSignals.Scheme == ControlScheme.Keyboard) return;
            if (onChangedRequest)
            {
                onChangedRequest = false;
                SetNewControlSchem(InputSignals.Scheme);
            }
            if (eventSystem != null && !eventSystem.alreadySelecting)
            {
                if (CurrentSelected != null && !CurrentSelected.CanBeSelected)
                {
                    OnChanged();
                }
                else if (CurrentSelected == null)
                {
                    eventSystem.SetSelectedGameObject(null);
                }
                else if (eventSystem.currentSelectedGameObject != CurrentSelected.gameObject)
                {
                    eventSystem.SetSelectedGameObject(CurrentSelected.gameObject);
                }
            }
        }

        internal void UpdateByInstance(SelectableBase instance)
        {
            if (instance == null) return;
            if (CurrentSelected == instance)
            {
                if (!instance.CanBeSelected)
                    OnChanged();
            }
            else if (instance.CanBeSelected)
            {
                if (CurrentSelected == null || !CurrentSelected.CanBeSelected || instance.Priority > CurrentSelected.Priority)
                    OnChanged();
            }
        }
    }
}
