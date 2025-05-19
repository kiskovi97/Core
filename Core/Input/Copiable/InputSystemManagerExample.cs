using Kiskovi.Core;

using UnityEngine;

using Zenject;

namespace PuzzleProject.Core
{
    internal class InputSystemManagerExample : MonoBehaviour
    {
        private static InputSystemManagerExample Instance;
        private static InputActionsExample inputActions;

        [Inject] private UISignalSenderExample uiInteractionSender;
        [Inject] private PlayerSignalSenderExample playerSignalSender;

        private static void Initalize(InputSystemManagerExample instance, UISignalSenderExample uiInteractionSender, 
            PlayerSignalSenderExample playerSignalSender)
        {
            Instance = instance;

            inputActions = new InputActionsExample();
            inputActions.Enable();
            inputActions.UIInputs.SetCallbacks(uiInteractionSender);
            inputActions.Player.SetCallbacks(playerSignalSender);
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
                Initalize(this, uiInteractionSender, playerSignalSender);
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
