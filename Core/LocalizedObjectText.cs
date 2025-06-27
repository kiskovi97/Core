using System;

using Newtonsoft.Json.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Kiskovi.Core
{
    [RequireComponent(typeof(TMP_Text))]
    internal class LocalizedObjectText : MonoBehaviour
    {
        public LocalizedString locale;
        public string key;
        private TMP_Text text;

        private void OnEnable()
        { 
            LocalizationSettings.SelectedLocaleChanged += RefreshLocalization;
            Refresh();
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= RefreshLocalization;
        }

        protected void RefreshLocalization(Locale locale)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (text == null)
                text = GetComponent<TMP_Text>();
            text.text = GetLocaleData(locale, key);
        }

        public static string GetLocaleData(LocalizedString localizedString, string key)
        {
            if (localizedString != null && localizedString.IsEmpty && LocalizationSettings.Instance != null) return "";
            try
            {
                var jsonString = localizedString.GetLocalizedString();
                var data =  JObject.Parse(jsonString);
                return data[key].ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
