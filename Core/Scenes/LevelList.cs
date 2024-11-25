using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Kiskovi.Core
{
    [CreateAssetMenu(menuName = "ScriptableObjects/LevelList")]
    internal class LevelList : ScriptableObject
    {
        [AssetReferenceUILabelRestriction("scene")]
        public List<AssetReference> SCENES;

        internal AssetReference GetScene(int sceneIndex)
        {
            if (SCENES.Length > sceneIndex)
                return SCENES[sceneIndex];
            return null;
        }
    }
}
