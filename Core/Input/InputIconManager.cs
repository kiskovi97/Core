using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    [Serializable]
    public struct KeyboardInputIcons
    {
        public Sprite A;
        public Sprite W;
        public Sprite S;
        public Sprite D;
        public Sprite E;
        public Sprite Q;
        public Sprite Space;
        public Sprite Enter;
        public Sprite Ctrl;
        public Sprite Shift;
        public Sprite LeftClick;
        public Sprite RightClick;
        public Sprite MiddleClick;
        public Sprite MiddleScroll;
        public Sprite GetSprite(string controlPath)
        {
            // From the input system, we get the path of the control on device. So we can just
            // map from that to the sprites we have for gamepads.
            switch (controlPath)
            {
                case "a": return A;
                case "w": return W;
                case "s": return S;
                case "d": return D;
                case "e": return E;
                case "q": return Q;
                case "space": return Space;
                case "enter": return Enter;
                case "control": return Ctrl;
                case "shift": return Shift;
                case "leftButton": return LeftClick;
                case "rightButton": return RightClick;
                case "middleButton": return MiddleClick;
                case "scroll": return MiddleScroll;
            }
            Debug.LogWarning(controlPath + " has no icon");
            return null;
        }
    }

    [Serializable]
    public struct ControllerInputIcons
    {
        public Sprite buttonSouth;
        public Sprite buttonNorth;
        public Sprite buttonEast;
        public Sprite buttonWest;
        public Sprite startButton;
        public Sprite selectButton;
        public Sprite leftTrigger;
        public Sprite rightTrigger;
        public Sprite leftShoulder;
        public Sprite rightShoulder;
        public Sprite dpad;
        public Sprite dpadUp;
        public Sprite dpadDown;
        public Sprite dpadLeft;
        public Sprite dpadRight;
        public Sprite leftStick;
        public Sprite rightStick;
        public Sprite leftStickPress;
        public Sprite rightStickPress;

        public Sprite GetSprite(string controlPath)
        {
            // From the input system, we get the path of the control on device. So we can just
            // map from that to the sprites we have for gamepads.
            switch (controlPath)
            {
                case "buttonSouth": return buttonSouth;
                case "buttonNorth": return buttonNorth;
                case "buttonEast": return buttonEast;
                case "buttonWest": return buttonWest;
                case "start": return startButton;
                case "select": return selectButton;
                case "leftTrigger": return leftTrigger;
                case "rightTrigger": return rightTrigger;
                case "leftShoulder": return leftShoulder;
                case "rightShoulder": return rightShoulder;
                case "dpad": return dpad;
                case "dpad/up": return dpadUp;
                case "dpad/down": return dpadDown;
                case "dpad/left": return dpadLeft;
                case "dpad/right": return dpadRight;
                case "leftStick": return leftStick;
                case "rightStick": return rightStick;
                case "leftStickPress": return leftStickPress;
                case "rightStickPress": return rightStickPress;
            }
            Debug.LogWarning(controlPath +" has no icon");
            return null;
        }
    }

    public interface IInputIconManager
    {
        public Sprite GetSprite(InputActionReference reference);
    }

    [Serializable]
    public struct InputIconSettings
    {
        public ControllerInputIcons xboxIcons;
        public KeyboardInputIcons keyboard;
    }

    internal class InputIconManager : IInputIconManager
    {
        private InputIconSettings _icons;

        public InputIconManager(InputIconSettings icons)
        {
            _icons = icons;
        }

        public Sprite GetSprite(InputActionReference reference)
        {
            if (reference == null || reference.action == null)
            {
                return null;
            }

            foreach (var binding in reference.action.bindings)
            {
                // Skip if it's not a part of a control scheme
                if (binding.groups.Contains(InputSignals.SchemeName))
                {
                    foreach (var device in InputSystem.devices)
                    {
                        var control = InputControlPath.TryFindControl(device, binding.effectivePath);
                        if (control != null)
                        {
                            // Get the control path part (e.g., "buttonSouth")
                            var shortPath = control.path.Substring(control.path.LastIndexOf('/') + 1);

                            switch (InputSignals.Scheme)
                            {
                                case ControlScheme.XboxController:
                                    return _icons.xboxIcons.GetSprite(shortPath);
                                case ControlScheme.Keyboard:
                                    return _icons.keyboard.GetSprite(shortPath);
                                default:
                                    return null;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
