using UnityEngine;
using UnityEngine.Localization;

namespace Kiskovi.Core
{
    [CreateAssetMenu(fileName = "Tutorial", menuName = "KiskoviCore/Tutorial")]
    public class TutorialReference : ScriptableObject, IData
    {
        public TutorialReference[] dependencies;
        public string key => name;
        public LocalizedString localizedReference;
        public Sprite iconSprite;
    }
}
