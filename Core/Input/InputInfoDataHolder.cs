using TMPro;
using Zenject;

namespace Kiskovi.Core
{
    internal class InputInfoDataHolder : LocalizedDataHolder<InputInfoGroup>
    {
        public TMP_Text text;
        public InputIconList iconList;

        [Inject]
        private IInputIconManager iconManager;

        public override void SetData(IData itemData)
        {
            base.SetData(itemData);

            if (Data == null)
                return;

            if (text != null)
                text.text = GetLocalizedString(Data.title);

            var icons = iconManager.GetIconData(Data.inputActionReference);

            if (iconList != null)
            {
                iconList.UpdateList(icons);
            }
        }
    }
}
