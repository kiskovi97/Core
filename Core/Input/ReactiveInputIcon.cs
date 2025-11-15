using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Zenject;

namespace Kiskovi.Core
{
    internal class ReactiveInputIcon : MonoBehaviour
    {
        public SpriteRenderer iconSprite;
        public InputActionReference inputActionReference;
        public Image iconImage;
        public Sprite defaultIconSprite;
        public TMP_Text inputText;

        [Inject] private IInputIconManager _iconManager;
        [Inject] private SignalBus _signalBus;

        private void OnEnable()
        {
            UpdateValue();
            LocalizationSettings.SelectedLocaleChanged += RefreshLocalization;
            _signalBus.Subscribe<InputSignals.ControlSchemeChanged>(OnControlSchemeChanged);
        }

        private void OnControlSchemeChanged()
        {
            UpdateValue();
        }

        private void RefreshLocalization(Locale locale)
        {
            UpdateValue();
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= RefreshLocalization;
            _signalBus.TryUnsubscribe<InputSignals.ControlSchemeChanged>(OnControlSchemeChanged);
        }

        public void UpdateValue()
        {
            var icon = _iconManager.GetSprite(inputActionReference);

            if (iconSprite != null)
                iconSprite.sprite = icon != null ? icon : defaultIconSprite;

            if (iconImage != null)
                iconImage.sprite = icon != null ? icon : defaultIconSprite;



            if (inputText != null)
            {
                if (icon != null)
                    inputText.text = "";
                else
                    inputText.text = _iconManager.GetString(inputActionReference);
            }
        }
    }
}
