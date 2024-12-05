using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

namespace Kiskovi.Core
{
    internal class InputSystemManager : MonoBehaviour
    {
        private static InputSystemManager Instance;
        private static BasicInputActions inputActions;

        [Inject] private UISignalSender uiInteractionSender;
        [Inject] private MovementSignalSender movementSender;
        [Inject] private InteractionSignalSender interactionSender;

        private static void Initalize(InputSystemManager instance, UISignalSender uiInteractionSender, 
            MovementSignalSender movementSignalSender, InteractionSignalSender interactionSignalSender)
        {
            Instance = instance;

            inputActions = new BasicInputActions();
            inputActions.Enable();
            inputActions.UIInputs.SetCallbacks(uiInteractionSender);
            inputActions.Movement.SetCallbacks(movementSignalSender);
            inputActions.Interaction.SetCallbacks(interactionSignalSender);
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
                Initalize(this, uiInteractionSender, movementSender, interactionSender);
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
