using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

using Newtonsoft.Json.Linq;

namespace Kiskovi.Core
{
    public class LocalizedDataHolder<T> : DataHolder<T> where T : class, IData
    {
        protected virtual void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += RefreshLocalization;
            Refresh();
        }

        protected virtual void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= RefreshLocalization;
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
