using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

using Newtonsoft.Json.Linq;
using Zenject;
using System;

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

        protected string GetLocaleData(LocalizedString localizedString, string key)
        {
            return LocalizedObjectText.GetLocaleData(localizedString, key);
        }
    }
}
