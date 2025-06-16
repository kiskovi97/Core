using UnityEngine;
using UnityEngine.InputSystem;

namespace PuzzleProject.Core
{
    internal class DontDestroyObject : MonoBehaviour
    {
        private static DontDestroyObject Instance;

        private static void Initalize(DontDestroyObject instance)
        {
            Instance = instance;
        }

        private static void ResetInstance()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Initalize(this);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                ResetInstance();
            }
        }
    }
}
