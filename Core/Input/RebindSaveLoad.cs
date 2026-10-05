using Kiskovi.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset actions;

    private string RebindsKey => "rebinds_" + actions.name;

    [Inject]
    private SignalBus _signalBus;

    public void OnEnable()
    {
        if (actions == null)
            return;

        _signalBus.Subscribe<BindingChangedSignal>(OnBindingChanged);

        var rebinds = PlayerPrefs.GetString(RebindsKey);
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);
    }

    private void OnBindingChanged()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(RebindsKey, rebinds);
        PlayerPrefs.Save();
    }

    public void OnDisable()
    {
        if (actions == null)
            return;

        _signalBus.Unsubscribe<BindingChangedSignal>(OnBindingChanged);

        OnBindingChanged();
    }
}
