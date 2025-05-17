using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

using Newtonsoft.Json.Linq;
using Zenject;

namespace Kiskovi.Core
{
    public class LocalizedDataHolder<T> : DataHolder<T> where T : class, IData
    {
        [Inject] protected SignalBus _signalBus;

        protected virtual void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += RefreshLocalization;
            _signalBus.Subscribe<InputSignals.ControlSchemeChanged>(OnControlSchemeChanged);
            Refresh();
        }

        protected virtual void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= RefreshLocalization;
            _signalBus.Unsubscribe<InputSignals.ControlSchemeChanged>(OnControlSchemeChanged);
        }

        private void OnControlSchemeChanged()
        {
            Refresh();
        }

        protected void RefreshLocalization(Locale locale)
        {
            Refresh();
        }

        protected JObject GetLocaleData(LocalizedString localizedString)
        {
            var jsonString = localizedString.GetLocalizedString();
            return JObject.Parse(jsonString);
        }
    }
}
