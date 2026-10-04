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
        public TMP_Text titleText;
        public InputIconList iconList;

        [Inject]
        private ITutorialManager _manager;

        [Inject]
        private IInputIconManager iconManager;

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
                var icons = Data.GetIcon(iconManager);

                if (iconList != null)
                {
                    iconList.UpdateList(icons);
                }

                if (titleText != null)
                {
                    titleText.text = GetLocalizedString(Data.TitleString);
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
            var isAvailable = _manager.IsTutorialAvailable(Data);

            switch (type)
            {
                case Type.HideOnCompleted:
                    objectBase.SetActive(isAvailable && !isComplete, instant);
                    break;
                case Type.ShowOnCompleted:
                    objectBase.SetActive(isAvailable && isComplete, instant);
                    break;
            }
        }
    }
}
