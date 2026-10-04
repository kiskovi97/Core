using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Kiskovi.Core
{
    public class InputIcon : DataHolder<IconData>
    {
        public SpriteRenderer iconSprite;
        public Image iconImage;
        public GameObject noIconObject;
        public TMP_Text inputText;

        public override void SetData(IData itemData)
        {
            base.SetData(itemData);

            if (iconSprite != null)
            {
                iconSprite.sprite = Data.sprite;
                iconSprite.gameObject.SetActive(Data.sprite != null);
            }

            if (iconImage != null)
            {
                iconImage.sprite = Data.sprite;
                iconImage.gameObject.SetActive(Data.sprite != null);
            }
            if (noIconObject != null)
                noIconObject.SetActive(Data.sprite == null);

            if (inputText != null)
            {
                if (Data.sprite != null)
                    inputText.text = "";
                else
                    inputText.text = Data.text;
                inputText.gameObject.SetActive(Data.sprite == null);
            }
        }
    }
}
