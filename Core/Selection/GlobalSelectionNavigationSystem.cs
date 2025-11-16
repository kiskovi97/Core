using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{

    internal class GlobalSelectionNavigationSystem : IInitializable, IDisposable
    {
        private GlobalSelectionSystem _selectionSystem;

        private SignalBus _signalBus;

        public GlobalSelectionNavigationSystem(GlobalSelectionSystem selectionSystem, SignalBus signalBus)
        {
            _selectionSystem = selectionSystem;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<UIInteractions.Navigate>(OnNavigate);
            _signalBus.Subscribe<UIInteractions.NavigateUI>(OnNavigateUI);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<UIInteractions.Navigate>(OnNavigate);
            _signalBus.TryUnsubscribe<UIInteractions.NavigateUI>(OnNavigateUI);
        }

        private void OnNavigateUI(UIInteractions.NavigateUI signal)
        {
            var getSelectableList = _selectionSystem.GetCurrentLayerSelectables(false);
            var nextSelected = GetNextSelected(getSelectableList, signal.value);
            if (nextSelected != null)
            {
                _selectionSystem.SetSelectedGameObject(nextSelected);
            }
        }

        private void OnNavigate(UIInteractions.Navigate signal)
        {
            var getSelectableList = _selectionSystem.GetCurrentLayerSelectables(true);
            var nextSelected = GetNextSelected(getSelectableList, signal.value);
            if (nextSelected != null)
            {
                _selectionSystem.SetSelectedGameObject(nextSelected);
            }
        }

        private SelectableBase GetNextSelected(IEnumerable<SelectableBase> selectables, Vector2 goalDir)
        {
            if (!_selectionSystem.CanNavigate) return null;
            var selected = _selectionSystem.CurrentSelected;
            if (selected == null) return selectables.FirstOrDefault();

            SelectableBase nextSelected = null;
            foreach (var selection in selectables)
            {
                if (selected == selection) continue;

                var dir = selection.transform.position - selected.transform.position;

                var dot = GetValue(goalDir, dir);
                if (dot < 0 || dir.sqrMagnitude == 0)
                {
                    continue;
                }

                if (nextSelected == null)
                {
                    nextSelected = selection;
                    continue;
                }
                var prevDir = nextSelected.transform.position - selected.transform.position;
                var prevDot = GetValue(goalDir, prevDir);
                if (prevDot == dot)
                {
                    if (dir.magnitude < prevDir.magnitude)
                    {
                        nextSelected = selection;
                    }
                }
                else if (prevDot > dot)
                {
                    nextSelected = selection;
                }
            }
            return nextSelected;
        }

        private static float GetValue(Vector2 goalDir, Vector3 dir)
        {
            return Vector2.Dot(goalDir, dir) * (Mathf.Round(Vector2.Angle(goalDir, dir) / 45f) / 4f);
        }
    }
}
