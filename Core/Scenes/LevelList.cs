using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Kiskovi.Core
{
    [Serializable]
    public class LevelList
    {
        [AssetReferenceUILabelRestriction("scene")]
        public List<AssetReference> SCENES;

        internal AssetReference GetScene(int sceneIndex)
        {
            if (SCENES.Count > sceneIndex)
                return SCENES[sceneIndex];
            return null;
        }
    }
}
