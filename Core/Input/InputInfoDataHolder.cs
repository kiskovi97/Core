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

        [Inject] private IInputIconManager iconManager;

        public override void SetData(IData itemData)
        {
            base.SetData(itemData);

            if (Data == null) return;

            if (text != null)
                text.text = GetLocaleData(Data.referenceName, "title");

            var icon = iconManager.GetSprite(Data.inputActionReference);

            if (iconSprite != null)
                iconSprite.sprite = icon;

            if (iconImage != null)
                iconImage.sprite = icon;
        }
    }
}
