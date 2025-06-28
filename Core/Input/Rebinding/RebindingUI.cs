using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

namespace Kiskovi.Core
{
    public class BindingChangedSignal { }

    public class RebindingUI : MonoBehaviour
    {
        [Tooltip("Reference to action that is to be rebound from the UI.")]
        [SerializeField]
        private InputActionReference m_Action;

        [SerializeField]
        private string m_BindingId;

        [SerializeField]
        private InputBinding.DisplayStringOptions m_DisplayStringOptions;

        [SerializeField]
        private TMP_Text m_BindingText;

        private InputActionRebindingExtensions.RebindingOperation m_RebindOperation;

        private static List<RebindingUI> s_RebindActionUIs;

        [Inject] private SignalBus _signalBus;

        public void UpdateBindingDisplay()
        {
            var displayString = string.Empty;
            var deviceLayoutName = default(string);
            var controlPath = default(string);

            // Get display string from action.
            var action = m_Action?.action;
            if (action != null)
            {
                var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == m_BindingId);
                if (bindingIndex != -1)
                    displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, m_DisplayStringOptions);
            }

            // Set on label (if any).
            if (m_BindingText != null)
                m_BindingText.text = displayString;
        }

        public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
        {
            bindingIndex = -1;

            action = m_Action?.action;
            if (action == null)
                return false;

            if (string.IsNullOrEmpty(m_BindingId))
                return false;

            // Look up binding index.
            var bindingId = new Guid(m_BindingId);
            bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
            if (bindingIndex == -1)
            {
                Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
                return false;
            }

            return true;
        }

        public void ResetToDefault()
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex))
                return;

            if (action.bindings[bindingIndex].isComposite)
            {
                // It's a composite. Remove overrides from part bindings.
                for (var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                    action.RemoveBindingOverride(i);
            }
            else
            {
                action.RemoveBindingOverride(bindingIndex);
            }

            _signalBus.TryFire(new BindingChangedSignal());

            UpdateBindingDisplay();
        }

        public void StartInteractiveRebind()
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex))
                return;

            // If the binding is a composite, we need to rebind each part in turn.
            if (action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                    PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);
            }
            else
            {
                PerformInteractiveRebind(action, bindingIndex);
            }
        }

        private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            m_RebindOperation?.Cancel(); // Will null out m_RebindOperation.

            void CleanUp()
            {
                m_RebindOperation?.Dispose();
                m_RebindOperation = null;
            }

            action.Disable();

            string originalBindingPath = action.bindings[bindingIndex].effectivePath;

            // Configure the rebind.
            m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .OnCancel(
                    operation =>
                    {
                        UpdateBindingDisplay();
                        CleanUp();
                    })
                .OnComplete(
                    operation =>
                    {
                        _signalBus.TryFire(new BindingChangedSignal());

                        UpdateBindingDisplay();
                        CleanUp();

                        // If there's more composite parts we should bind, initiate a rebind
                        // for the next part.
                        if (allCompositeParts)
                        {
                            var nextBindingIndex = bindingIndex + 1;
                            if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                                PerformInteractiveRebind(action, nextBindingIndex, true);
                        }

                        action.Enable();

                        string newBindingPath = action.bindings[bindingIndex].effectivePath;
                        SwapConflictingBindings(action, bindingIndex, newBindingPath, originalBindingPath);
                    });

            // If it's a part binding, show the name of the part in the UI.
            var partName = default(string);
            if (action.bindings[bindingIndex].isPartOfComposite)
                partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

            // If we have no rebind overlay and no callback but we have a binding text label,
            // temporarily set the binding text label to "<Waiting>".
            if (m_BindingText != null)
                m_BindingText.text = "<Waiting...>";

            m_RebindOperation.Start();
        }

        private void SwapConflictingBindings(InputAction currentAction, int currentBindingIndex, string newPath, string oldPath)
        {
            var currentMap = currentAction.actionMap;
            var asset = currentMap?.asset;
            if (asset == null) return;

            var changed = false;

            foreach (var map in asset.actionMaps)
            {
                if (map != currentAction.actionMap) continue;

                foreach (var action in map.actions)
                {
                    if (action == currentAction) continue;

                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        var binding = action.bindings[i];

                        if (binding.effectivePath == oldPath)
                        {
                            changed = true;
                            // Swap bindings
                            Debug.LogWarning($"Same binding carry over: '{currentAction.name}' <-> '{action.name}' for path: {newPath}");

                            action.ApplyBindingOverride(i, newPath);
                        }
                        else if (binding.effectivePath == newPath)
                        {
                            changed = true;
                            // Swap bindings
                            Debug.LogWarning($"Swapping binding: '{currentAction.name}' <-> '{action.name}' for path: {newPath}");

                            action.ApplyBindingOverride(i, oldPath);
                        }
                    }
                }
            }

            if (changed)
                _signalBus.TryFire(new BindingChangedSignal());
        }

        protected void OnEnable()
        {
            if (s_RebindActionUIs == null)
                s_RebindActionUIs = new List<RebindingUI>();
            s_RebindActionUIs.Add(this);
            if (s_RebindActionUIs.Count == 1)
                InputSystem.onActionChange += OnActionChange;
        }

        protected void OnDisable()
        {
            m_RebindOperation?.Dispose();
            m_RebindOperation = null;

            s_RebindActionUIs.Remove(this);
            if (s_RebindActionUIs.Count == 0)
            {
                s_RebindActionUIs = null;
                InputSystem.onActionChange -= OnActionChange;
            }
        }

        private static void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.BoundControlsChanged)
                return;

            var action = obj as InputAction;
            var actionMap = action?.actionMap ?? obj as InputActionMap;
            var actionAsset = actionMap?.asset ?? obj as InputActionAsset;

            for (var i = 0; i < s_RebindActionUIs.Count; ++i)
            {
                var component = s_RebindActionUIs[i];
                var referencedAction = component.m_Action?.action;
                if (referencedAction == null)
                    continue;

                if (referencedAction == action ||
                    referencedAction.actionMap == actionMap ||
                    referencedAction.actionMap?.asset == actionAsset)
                    component.UpdateBindingDisplay();
            }
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            UpdateBindingDisplay();
        }
#endif
    }
}
