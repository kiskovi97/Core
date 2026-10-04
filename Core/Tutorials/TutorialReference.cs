using System;
using System.Collections.Generic;
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
        public InputInfoGroup[] inputInfoGroups;

        public IEnumerable<IconData> GetIcon(IInputIconManager iconManager)
        {
            var iconData = new List<IconData>();
            if (iconSprite != null)
                return new List<IconData>
                {
                    new IconData { sprite = iconSprite, text = "" },
                };

            if (inputInfoGroups != null)
            {
                foreach (var group in inputInfoGroups)
                {
                    iconData.AddRange(iconManager.GetIconData(group.inputActionReference));
                }
            }

            return iconData;
        }

        public LocalizedString TitleString
        {
            get
            {
                if (title.isDirty)
                    return inputInfoGroups.Length > 0 ? inputInfoGroups[0].title : title;
                return title;
            }
        }
    }
}
