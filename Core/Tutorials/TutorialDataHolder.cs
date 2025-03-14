using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kiskovi.Core
{
    internal class TutorialDataHolder : LocalizedDataHolder<TutorialReference>
    {
        public enum Type
        {
            HideOnCompleted,
            ShowOnCompleted,
        }

        public TutorialReference defaultValue;
        public Type type;
        public AnimatedObject objectBase;
        public SpriteRenderer iconSpriteRenderer;
        public Image iconImage;
        public TMP_Text titleText;
        public TMP_Text descriptionText;

        [Inject] private ITutorialManager _manager;

        protected void Start()
        {
            if (defaultValue != null)
            {
                SetData(defaultValue);
            }
            _manager.onChanged += OnAvailablilityChanged;
            OnAvailablilityChanged(true);
        }

        protected void OnDestroy()
        {
            _manager.onChanged -= OnAvailablilityChanged;
        }

        public override void SetData(IData itemData)
        {
            base.SetData(itemData);

            if (Data != null)
            {

                if (iconSpriteRenderer != null)
                    iconSpriteRenderer.sprite = Data.iconSprite;

                if (iconImage != null) 
                    iconImage.sprite = Data.iconSprite;

                var data = GetLocaleData(Data.localizedReference);

                if (titleText != null)
                {
                    titleText.text = data["title"].ToString();
                }
                if (descriptionText != null)
                {
                    descriptionText.text = data["description"].ToString();
                }

                OnAvailablilityChanged();
            }
        }
        private void OnAvailablilityChanged()
        {
            OnAvailablilityChanged(false);
        }

        private void OnAvailablilityChanged(bool instant)
        {
            var isComplete = _manager.IsTutorialComplete(Data);

            switch(type)
            {
                case Type.HideOnCompleted:
                    objectBase.SetActive(!isComplete, instant);
                    break;
                case Type.ShowOnCompleted:
                    objectBase.SetActive(isComplete, instant);
                    break;
            }
        }
    }
}
