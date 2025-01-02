using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    internal class InputSystemManager : MonoBehaviour
    {
        private static InputSystemManager Instance;
        private static BasicInputActions inputActions;

        [Inject] private UISignalSender uiInteractionSender;
        [Inject] private PlayerSignalSender playerSignalSender;
        [Inject] private MapSignalSender mapSignalSender;

        private static void Initalize(InputSystemManager instance, UISignalSender uiInteractionSender, 
            PlayerSignalSender playerSignalSender, MapSignalSender mapSignalSender)
        {
            Instance = instance;

            inputActions = new BasicInputActions();
            inputActions.Enable();
            inputActions.UIInputs.SetCallbacks(uiInteractionSender);
            inputActions.Player.SetCallbacks(playerSignalSender);
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
                Initalize(this, uiInteractionSender, playerSignalSender, mapSignalSender);
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
