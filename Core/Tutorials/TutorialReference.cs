using UnityEngine;
using UnityEngine.Localization;

namespace Kiskovi.Core
{
    [CreateAssetMenu(fileName = "Tutorial", menuName = "KiskoviCore/Tutorial")]
    public class TutorialReference : ScriptableObject, IData
    {
        public TutorialReference[] dependencies = new TutorialReference[0];
        public string key => name;
        public LocalizedString localizedReference;
        public Sprite iconSprite;
    }
}
