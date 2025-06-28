using Kiskovi.Core;

using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset actions;

    [Inject] private SignalBus _signalBus;

    public void OnEnable()
    {
        if (actions == null) return;

        _signalBus.Subscribe<BindingChangedSignal>(OnBindingChanged);

        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);
    }

    private void OnBindingChanged()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public void OnDisable()
    {
        if (actions == null) return;

        _signalBus.Unsubscribe<BindingChangedSignal>(OnBindingChanged);

        OnBindingChanged();
    }
}
