using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kiskovi.Core
{
    internal class InputInfoDataHolder : LocalizedDataHolder<InputInfo>
    {
        public TMP_Text text;
        public SpriteRenderer iconSprite;
        public Image iconImage;

        public override void SetData(IData itemData)
        {
            base.SetData(itemData);

            if (Data == null) return;

            if (text != null)
                text.text = Data.referenceName.GetLocalizedString();

            if (iconSprite != null)
                iconSprite.sprite = Data.icon;

            if (iconImage != null)
                iconImage.sprite = Data.icon;
        }
    }
}
