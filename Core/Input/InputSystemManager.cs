using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    internal class InputSystemManager : MonoBehaviour
    {
        private static InputSystemManager Instance;
        private static BasicInputActions inputActions;

        [Inject] private UISignalSender uiInteractionSender;

        private static void Initalize(InputSystemManager instance, UISignalSender uiInteractionSender)
        {
            Instance = instance;

            inputActions = new BasicInputActions();
            inputActions.Enable();
            inputActions.UIInputs.SetCallbacks(uiInteractionSender);
        }

        private static void ResetInstance()
        {
            inputActions.Disable();
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Initalize(this, uiInteractionSender);
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
