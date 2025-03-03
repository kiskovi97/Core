using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

using UnityEngine.UI;

using UnityEngine;

namespace Kiskovi.Core
{
    internal class LocaleSelectionBox : MonoBehaviour
    {
        public SelectionBox dropdown;

        IEnumerator Start()
        {
            // Wait for the localization system to initialize
            var operation = LocalizationSettings.InitializationOperation;
            if (!operation.IsDone)
                yield return operation;

            // Generate list of available Locales
            var options = new List<Dropdown.OptionData>();
            int selected = 0;
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selected = i;
                options.Add(new Dropdown.OptionData(locale.name));
            }
            dropdown.SetOptions(options, selected);
            dropdown.onValueChanged.AddListener(LocaleSelected);
        }

        static void LocaleSelected(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
    }
}
