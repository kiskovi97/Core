using UnityEngine;

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
        [Inject] private MapSignalSender mapSignalSender;

        private static void Initalize(InputSystemManager instance, UISignalSender uiInteractionSender, 
            MovementSignalSender movementSignalSender, InteractionSignalSender interactionSignalSender, MapSignalSender mapSignalSender)
        {
            Instance = instance;

            inputActions = new BasicInputActions();
            inputActions.Enable();
            inputActions.UIInputs.SetCallbacks(uiInteractionSender);
            inputActions.Movement.SetCallbacks(movementSignalSender);
            inputActions.Interaction.SetCallbacks(interactionSignalSender);
            inputActions.Map.SetCallbacks(mapSignalSender);
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
                Initalize(this, uiInteractionSender, movementSender, interactionSender, mapSignalSender);
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
