using System;
using System.Collections.Generic;
using System.Linq;
using Kiskovi.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class RebindSaveLoad
{
    private readonly InputActionAsset[] _actions;

    private string RebindsKey(InputActionAsset action)
    {
        return "rebinds_" + action.name;
    }

    public RebindSaveLoad(SignalBus signalBus, IEnumerable<InputActionAsset> actions)
    {
        _actions = actions.ToArray();
        signalBus.Subscribe<BindingChangedSignal>(OnBindingChanged);

        foreach (var action in _actions)
        {
            if (action == null)
            {
                continue;
            }

            var rebinds = PlayerPrefs.GetString(RebindsKey(action), string.Empty);
            if (!string.IsNullOrEmpty(rebinds))
            {
                action.LoadBindingOverridesFromJson(rebinds);
            }
        }

        SynchronizeActivePlayerInputs();
    }

    public void SynchronizeRuntimeAsset(InputActionAsset runtimeAsset)
    {
        if (runtimeAsset == null)
            return;

        var canonicalAsset = FindCanonicalAsset(runtimeAsset);
        if (canonicalAsset == null || canonicalAsset == runtimeAsset)
            return;

        ReplaceOverrides(runtimeAsset, canonicalAsset.SaveBindingOverridesAsJson());
    }

    private void OnBindingChanged(BindingChangedSignal signal)
    {
        var runtimeAsset = signal?.Asset;
        if (runtimeAsset == null)
        {
            return;
        }

        var canonicalAsset = FindCanonicalAsset(runtimeAsset, signal.BindingId);
        if (canonicalAsset == null)
        {
            return;
        }

        var runtimeOverrides = runtimeAsset.SaveBindingOverridesAsJson();
        ReplaceOverrides(canonicalAsset, runtimeOverrides);

        var savedOverrides = canonicalAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(RebindsKey(canonicalAsset), savedOverrides);
        PlayerPrefs.Save();

        if (runtimeAsset != canonicalAsset)
            ReplaceOverrides(runtimeAsset, savedOverrides);

        SynchronizeActivePlayerInputs(canonicalAsset, savedOverrides);

        var overriddenBindings = canonicalAsset
            .actionMaps.SelectMany(map => map.bindings)
            .Where(binding => binding.hasOverrides)
            .Select(binding =>
                $"action='{binding.action}', id={binding.id}, path='{binding.effectivePath}'"
            )
            .ToArray();
    }

    private InputActionAsset FindCanonicalAsset(
        InputActionAsset runtimeAsset,
        Guid? bindingId = null
    )
    {
        if (runtimeAsset == null)
            return null;

        return _actions.FirstOrDefault(action =>
            action != null
            && HaveSameActionMaps(action, runtimeAsset)
            && (
                !bindingId.HasValue
                || action
                    .actionMaps.SelectMany(map => map.bindings)
                    .Any(binding => binding.id == bindingId.Value)
            )
        );
    }

    private void SynchronizeActivePlayerInputs(
        InputActionAsset canonicalAsset = null,
        string overrideJson = null
    )
    {
        foreach (var playerInput in PlayerInput.all)
        {
            if (playerInput == null || playerInput.actions == null)
                continue;

            var matchingAsset =
                canonicalAsset == null ? FindCanonicalAsset(playerInput.actions)
                : HaveSameActionMaps(playerInput.actions, canonicalAsset) ? canonicalAsset
                : null;
            if (matchingAsset == null || matchingAsset == playerInput.actions)
                continue;

            ReplaceOverrides(
                playerInput.actions,
                overrideJson ?? matchingAsset.SaveBindingOverridesAsJson()
            );
        }
    }

    private static bool HaveSameActionMaps(InputActionAsset first, InputActionAsset second)
    {
        if (first == null || second == null || first.actionMaps.Count == 0)
            return false;

        if (first.actionMaps.Count != second.actionMaps.Count)
            return false;

        var mapIds = new HashSet<Guid>(first.actionMaps.Select(map => map.id));
        return second.actionMaps.All(map => mapIds.Contains(map.id));
    }

    private static void ReplaceOverrides(InputActionAsset asset, string overrideJson)
    {
        foreach (var map in asset.actionMaps)
            map.RemoveAllBindingOverrides();

        if (!string.IsNullOrEmpty(overrideJson))
            asset.LoadBindingOverridesFromJson(overrideJson);
    }
}
