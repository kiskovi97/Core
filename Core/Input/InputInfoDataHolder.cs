using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kiskovi.Core
{
    internal class InputInfoDataHolder : LocalizedDataHolder<InputInfoGroup>
    {
        public TMP_Text text;
        public SpriteRenderer iconSprite;
        public Image iconImage;
        public Sprite defaultIconSprite;
        public TMP_Text inputText;

        [Inject] private IInputIconManager iconManager;

        public override void SetData(IData itemData)
        {
            base.SetData(itemData);

            if (Data == null) return;

            if (text != null)
                text.text = GetLocalizedString(Data.title);

            var icon = iconManager.GetSprite(Data.inputActionReference);

            if (iconSprite != null)
                iconSprite.sprite = icon != null ? icon : defaultIconSprite;

            if (iconImage != null)
                iconImage.sprite = icon != null ? icon : defaultIconSprite;



            if (inputText != null)
            {
                if (icon != null)
                    inputText.text = "";
                else
                    inputText.text = iconManager.GetString(Data.inputActionReference);
            }
        }
    }
}
