using System;

using UnityEngine.AddressableAssets;

namespace Kiskovi.Core
{
    internal class SceneProvider
    {
        internal SceneList levelList;

        internal SceneProvider(SceneList levelList)
        {
            this.levelList = levelList;
        }

        internal AssetReference GetScene(int sceneIndex)
        {
            return levelList.GetScene(sceneIndex);
        }
    }
}
