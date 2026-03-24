using System;
using Newtonsoft.Json.Linq;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;

namespace Kiskovi.Core
{
    public class LocalizedDataHolder<T> : DataHolder<T>
        where T : class, IData
    {
        [Inject]
        protected SignalBus _signalBus;

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

        public string GetLocalizedString(LocalizedString value)
        {
            if (value.IsEmpty)
                return "";

            return value.GetLocalizedString();
        }
    }
}
