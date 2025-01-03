using System;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Kiskovi.Core
{
    public enum SceneEnum
    {
        None = 0,
        Start_Menu = 10,
        Hub = 20,
        Level = 30,
    }

    [Serializable]
    public class SceneList
    {

        [AssetReferenceUILabelRestriction("scene")]
        public AssetReference START_MENU;

        [AssetReferenceUILabelRestriction("scene")]
        public AssetReference HUB;

        [AssetReferenceUILabelRestriction("scene")]
        public AssetReference LEVEL;

        internal AssetReference GetScene(SceneEnum scene)
        {
            switch (scene)
            {
                case SceneEnum.Start_Menu: return START_MENU;
                case SceneEnum.Hub: return HUB;
                case SceneEnum.Level: return LEVEL;
            }
            return null;
        }
    }
}
