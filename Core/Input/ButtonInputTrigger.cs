using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace Kiskovi.Core
{
    [RequireComponent(typeof(Button))]
    public class ButtonInputTrigger : MonoBehaviour
    {
        public GamepadButton button;

        static Gamepad virtualGamepad;

        void Awake()
        {
            if (virtualGamepad == null)
                virtualGamepad = InputSystem.AddDevice<Gamepad>();

            GetComponent<Button>().onClick.AddListener(PressButton);
        }

        void PressButton()
        {
            StartCoroutine(SimulatePress());
        }

        IEnumerator SimulatePress()
        {
            // Press
            InputSystem.QueueStateEvent(
                virtualGamepad,
                new GamepadState().WithButton(button, true)
            );
            InputSystem.Update();

            yield return new WaitForSeconds(0.05f);

            // Release
            InputSystem.QueueStateEvent(
                virtualGamepad,
                new GamepadState().WithButton(button, false)
            );
            InputSystem.Update();
        }
    }
}
