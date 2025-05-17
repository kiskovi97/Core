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
        public LocalizedString localizedReference;
        public Sprite iconSprite;
        public Override[] overrides = new Override[0];

        public LocalizedString LocalizedString
        {
            get
            {
                foreach (var over in overrides)
                {
                    if (InputSignals.Scheme == over.controlScheme)
                        return over.localizedReference;
                }
                return localizedReference;
            }
        }
        public Sprite IconSprite
        {
            get
            {
                foreach (var over in overrides)
                {
                    if (InputSignals.Scheme == over.controlScheme)
                        return over.iconSprite;
                }
                return iconSprite;
            }
        }
    }
}
