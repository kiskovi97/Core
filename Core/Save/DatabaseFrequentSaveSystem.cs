using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    internal class DatabaseFrequentSaveSystem : MonoBehaviour
    {
        public float FrequentTime = 5f;

        private float time;

        [Inject] private IDatabaseManager databaseManager;

        private void Update()
        {
            time += Time.deltaTime;
            if (time > FrequentTime)
            {
                time = 0f;
                databaseManager.SaveToDisk();

            }
        }
    }
}
