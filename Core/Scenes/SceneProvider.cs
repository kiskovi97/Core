using System;

using UnityEngine.AddressableAssets;

namespace Kiskovi.Core
{
    internal class SceneProvider
    {
        internal LevelList levelList;

        internal SceneProvider(LevelList levelList)
        {
            this.levelList = levelList;
        }

        internal AssetReference GetScene(int sceneIndex)
        {
            return levelList.GetScene(sceneIndex);
        }
    }
}
