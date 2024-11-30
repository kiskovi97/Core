using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Kiskovi.Core
{
    internal class InitialLoader : MonoBehaviour
    {
        [AssetReferenceUILabelRestriction("scene")]
        public AssetReference FIRSTSCENE;

        private void Start()
        {
            FIRSTSCENE.LoadSceneAsync(LoadSceneMode.Single);
        }
    }
}
