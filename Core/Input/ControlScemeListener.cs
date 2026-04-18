using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Kiskovi.Core
{
    public class PauseGameRequestSignal { }

    internal class ControlScemeListener : MonoBehaviour
    {
        private static ControlScemeListener instance;

        public PlayerInput playerInput;

        [Inject]
        private SignalBus _signalBus;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            playerInput.onControlsChanged += OnControlsChanged;
            playerInput.onDeviceLost += PlayerInput_onDeviceLost;
            OnControlsChanged(playerInput);
        }

        private void PauseGameRequest()
        {
            _signalBus.TryFire(new PauseGameRequestSignal());
        }

        private void PlayerInput_onDeviceLost(PlayerInput obj)
        {
            PauseGameRequest();
        }

        private void Start()
        {
            OnControlsChanged(playerInput);
        }

        private void OnEnable()
        {
            if (instance == this)
                OnControlsChanged(playerInput);
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
                playerInput.onControlsChanged -= OnControlsChanged;
                playerInput.onDeviceLost -= PlayerInput_onDeviceLost;
            }
        }

        private void OnControlsChanged(PlayerInput obj)
        {
            if (instance == this)
                switch (obj.currentControlScheme)
                {
                    case InputSignals.KEYBOARD_NAME:
                        if (InputSignals.Scheme != ControlScheme.Keyboard)
                            PauseGameRequest();
                        InputSignals.Scheme = ControlScheme.Keyboard;
                        _signalBus.TryFire(
                            new InputSignals.ControlSchemeChanged(ControlScheme.Keyboard)
                        );
                        break;
                    case InputSignals.XBOX_NAME:
                        if (InputSignals.Scheme != ControlScheme.XboxController)
                            PauseGameRequest();
                        InputSignals.Scheme = ControlScheme.XboxController;
                        _signalBus.TryFire(
                            new InputSignals.ControlSchemeChanged(ControlScheme.XboxController)
                        );
                        break;
                    case InputSignals.TOUCH_NAME:
                        if (InputSignals.Scheme != ControlScheme.Touch)
                            PauseGameRequest();
                        InputSignals.Scheme = ControlScheme.Touch;
                        _signalBus.TryFire(
                            new InputSignals.ControlSchemeChanged(ControlScheme.Touch)
                        );
                        break;
                    default:
                        InputSignals.Scheme = ControlScheme.XboxController;
                        _signalBus.TryFire(
                            new InputSignals.ControlSchemeChanged(ControlScheme.XboxController)
                        );
                        break;
                }
        }
    }
}
