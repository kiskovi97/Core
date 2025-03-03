using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    internal class SelectionBox : Selectable, ISubmitHandler
    {
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        [SerializeField] private TriggerAction onChanged;
        [SerializeField] private bool IsClickEnabled = true;
        [NonSerialized] public UnityEvent<int> onValueChanged = new UnityEvent<int>();
        private int value;
        private List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        public Dropdown.OptionData Selected => options[value];


        protected override void Start()
        {
            base.Start();
            if (nextButton != null )
            {
                nextButton.onClick.AddListener(OnNextClick);
                nextButton.interactable = value < options.Count - 1;
            }
            if (prevButton != null)
            {
                prevButton.onClick.AddListener(OnPrevClick);
                prevButton.interactable = value > 0;
            }
        }

        protected override void OnEnable()
        {
            LocalizationSettings.Instance.OnSelectedLocaleChanged += TranslationChanged;
            TranslationChanged();
        }

        protected override void OnDisable()
        {
            LocalizationSettings.Instance.OnSelectedLocaleChanged -= TranslationChanged;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (IsClickEnabled)
                OnNextClick();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (IsClickEnabled)
                OnNextClick();
        }

        public void SetOptions(IEnumerable<Dropdown.OptionData> options, int index = 0)
        {
            this.options.Clear();
            this.options.AddRange(options);
            value = index;
            SetValue();
        }

        private void OnPrevClick()
        {
            if (!interactable) return;
            TriggerAction.Trigger(onChanged);
            value--;
            SetValue();
        }

        private void OnNextClick()
        {
            if (!interactable) return;
            TriggerAction.Trigger(onChanged);
            value++;
            SetValue();
        }

        private void SetValue()
        {
            if (value < 0)
                value += options.Count;
            if (value >= options.Count)
                value -= options.Count;
            TranslationChanged();

            if (nextButton != null)
            {
                nextButton.interactable = value < options.Count - 1;
            }
            if (prevButton != null)
            {
                prevButton.interactable = value > 0;
            }

            onValueChanged.Invoke(value);
        }

        private void TranslationChanged(Locale locale = null)
        {
            if (options.Count == 0) return;

            var option = options[value];
            if (descriptionText != null)
                descriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString(option.text);
        }
    }
}
