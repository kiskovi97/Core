using System.Linq;
using UnityEngine;

namespace Kiskovi.Core
{
    internal class SceneFilteredObject : MonoBehaviour
    {
        public SceneEnum[] enabledScenes;

        public void OnEnable()
        {
            var isSceneLoaded = enabledScenes.Any(item => SceneLoader.IsSceneLoaded(item));
            if (!isSceneLoaded)
                Destroy(gameObject);
        }
    }
}
