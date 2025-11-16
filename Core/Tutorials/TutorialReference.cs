using System;

using UnityEngine;
using UnityEngine.Localization;

namespace Kiskovi.Core
{
    [Serializable]
    public class Override
    {
        public ControlScheme controlScheme;
        public LocalizedString localizedReference;
        public Sprite iconSprite;
    }

    [CreateAssetMenu(fileName = "Tutorial", menuName = "KiskoviCore/Tutorial")]
    public class TutorialReference : ScriptableObject, IData
    {
        public TutorialReference[] dependencies = new TutorialReference[0];
        public string key => name;
        public LocalizedString title;
        public Sprite iconSprite;
        public InputInfoGroup inputInfoGroup;

        public Sprite GetIcon(IInputIconManager iconManager)
        {
            if (iconSprite != null) return iconSprite;

            return iconManager.GetSprite(inputInfoGroup?.inputActionReference);
        }

        public LocalizedString TitleString
        {
            get
            {
                if (title.isDirty)
                    return inputInfoGroup?.title;
                return title;
            }
        }
    }
}
