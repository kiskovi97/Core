using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kiskovi.Core
{
    [Serializable]
    public struct KeyboardInputIcons
    {
        public Sprite wasd;
        public Sprite A;
        public Sprite W;
        public Sprite S;
        public Sprite D;
        public Sprite E;
        public Sprite Q;
        public Sprite M;
        public Sprite C;
        public Sprite Space;
        public Sprite Backspace;
        public Sprite Enter;
        public Sprite Ctrl;
        public Sprite Shift;
        public Sprite LeftClick;
        public Sprite RightClick;
        public Sprite MiddleClick;
        public Sprite MiddleScroll;
        public Sprite Up;
        public Sprite Down;
        public Sprite Left;
        public Sprite Right;

        public Sprite GetSprite(string controlPath)
        {
            // From the input system, we get the path of the control on device. So we can just
            // map from that to the sprites we have for gamepads.
            switch (controlPath)
            {
                case "wasd":
                    return wasd;
                case "a":
                    return A;
                case "w":
                    return W;
                case "s":
                    return S;
                case "d":
                    return D;
                case "e":
                    return E;
                case "q":
                    return Q;
                case "m":
                    return M;
                case "c":
                    return C;
                case "space":
                    return Space;
                case "backspace":
                    return Backspace;
                case "enter":
                    return Enter;
                case "control":
                    return Ctrl;
                case "shift":
                    return Shift;
                case "leftButton":
                    return LeftClick;
                case "rightButton":
                    return RightClick;
                case "middleButton":
                    return MiddleClick;
                case "scroll":
                case "scrollWheel":
                case "scroll/y":
                    return MiddleScroll;
                case "upArrow":
                    return Up;
                case "downArrow":
                    return Down;
                case "leftArrow":
                    return Left;
                case "rightArrow":
                    return Right;
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
                case "buttonSouth":
                    return buttonSouth;
                case "buttonNorth":
                    return buttonNorth;
                case "buttonEast":
                    return buttonEast;
                case "buttonWest":
                    return buttonWest;
                case "start":
                    return startButton;
                case "select":
                    return selectButton;
                case "leftTrigger":
                    return leftTrigger;
                case "rightTrigger":
                    return rightTrigger;
                case "leftShoulder":
                    return leftShoulder;
                case "rightShoulder":
                    return rightShoulder;
                case "dpad":
                    return dpad;
                case "dpad/up":
                    return dpadUp;
                case "dpad/down":
                    return dpadDown;
                case "dpad/left":
                    return dpadLeft;
                case "dpad/right":
                    return dpadRight;
                case "leftStick":
                    return leftStick;
                case "rightStick":
                    return rightStick;
                case "leftStickPress":
                    return leftStickPress;
                case "rightStickPress":
                    return rightStickPress;
            }
            Debug.LogWarning(controlPath + " has no icon");
            return null;
        }
    }

    public class IconData : IData
    {
        public Sprite sprite;
        public string text;
    }

    public interface IInputIconManager
    {
        Sprite GetSprite(InputActionReference reference);
        IEnumerable<IconData> GetIconData(InputActionReference reference);
        IconData GetIconData(InputActionReference reference, InputBinding inputBinding);
        string GetString(InputActionReference reference);
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

        public string GetString(InputActionReference reference)
        {
            if (reference == null || reference.action == null)
            {
                return null;
            }
            var bindings = reference.action.bindings;

            for (int i = 0; i < bindings.Count; i++)
            {
                var binding = bindings[i];
                // Skip if it's not a part of a control scheme
                if (binding.groups.Contains(InputSignals.SchemeName))
                {
                    foreach (var device in InputSystem.devices)
                    {
                        var control = InputControlPath.TryFindControl(
                            device,
                            binding.effectivePath
                        );
                        if (control != null)
                        {
                            // Return a user-facing display string
                            return reference.action.GetBindingDisplayString(i);
                        }
                    }
                }
            }
            return null;
        }

        public Sprite GetSprite(InputActionReference reference)
        {
            var sprites = GetIconData(reference);
            return sprites.FirstOrDefault()?.sprite;
        }

        public IconData GetIconData(InputActionReference reference, InputBinding binding)
        {
            var iconData = new List<IconData>();
            foreach (var device in InputSystem.devices)
            {
                var control = InputControlPath.TryFindControl(device, binding.effectivePath);
                if (control != null)
                {
                    // Get the control path part (e.g., "buttonSouth")
                    var path = control.path.Split('/');
                    var shortPath = path.Last();
                    if (shortPath == "y" || shortPath == "x")
                    {
                        shortPath = path[path.Length - 2];
                    }

                    switch (InputSignals.Scheme)
                    {
                        case ControlScheme.XboxController:
                        case ControlScheme.Touch:
                            var spriteXbox = _icons.xboxIcons.GetSprite(shortPath);
                            AddIconData(iconData, spriteXbox, reference, binding, shortPath);
                            break;
                        case ControlScheme.Keyboard:
                            var spriteKeyboard = _icons.keyboard.GetSprite(shortPath);
                            AddIconData(iconData, spriteKeyboard, reference, binding, shortPath);
                            break;
                    }
                }
            }
            MergeIfNeeded(iconData);
            return iconData.FirstOrDefault();
        }

        public IEnumerable<IconData> GetIconData(InputActionReference reference)
        {
            var iconData = new List<IconData>();
            if (reference == null || reference.action == null)
            {
                return iconData;
            }

            foreach (var binding in reference.action.bindings)
            {
                // Skip if it's not a part of a control scheme
                if (binding.groups.Contains(InputSignals.SchemeName))
                {
                    foreach (var device in InputSystem.devices)
                    {
                        var control = InputControlPath.TryFindControl(
                            device,
                            binding.effectivePath
                        );
                        if (control != null)
                        {
                            // Get the control path part (e.g., "buttonSouth")
                            var path = control.path.Split('/');
                            var shortPath = path.Last();
                            if (shortPath == "y" || shortPath == "x")
                            {
                                shortPath = path[path.Length - 2];
                            }

                            switch (InputSignals.Scheme)
                            {
                                case ControlScheme.XboxController:
                                case ControlScheme.Touch:
                                    var spriteXbox = _icons.xboxIcons.GetSprite(shortPath);
                                    AddIconData(
                                        iconData,
                                        spriteXbox,
                                        reference,
                                        binding,
                                        shortPath
                                    );
                                    break;
                                case ControlScheme.Keyboard:
                                    var spriteKeyboard = _icons.keyboard.GetSprite(shortPath);
                                    AddIconData(
                                        iconData,
                                        spriteKeyboard,
                                        reference,
                                        binding,
                                        shortPath
                                    );
                                    break;
                            }
                        }
                    }
                }
            }
            MergeIfNeeded(iconData);
            return iconData.Distinct();
        }

        private void MergeIfNeeded(List<IconData> iconData)
        {
            if (
                iconData.Any(item => item.text.ToLower() == "a")
                && iconData.Any(item => item.text.ToLower() == "d")
                && iconData.Any(item => item.text.ToLower() == "s")
                && iconData.Any(item => item.text.ToLower() == "w")
            )
            {
                iconData.RemoveAll(item =>
                    item.text.ToLower() == "a"
                    || item.text.ToLower() == "d"
                    || item.text.ToLower() == "s"
                    || item.text.ToLower() == "w"
                );
                iconData.Add(
                    new IconData { sprite = _icons.keyboard.GetSprite("wasd"), text = "WASD" }
                );
            }
        }

        private void AddIconData(
            List<IconData> iconData,
            Sprite sprite,
            InputActionReference reference,
            InputBinding binding,
            string shortPath
        )
        {
            var displayString = reference.action.GetBindingDisplayString(binding);
            if (string.IsNullOrEmpty(displayString))
            {
                displayString = shortPath;
            }
            if (iconData.Any(data => data.sprite == sprite))
            {
                var existingData = iconData.First(data => data.sprite == sprite);
                if (!existingData.text.Contains(displayString))
                    existingData.text += "/" + displayString;
                return;
            }
            else
            {
                iconData.Add(new IconData { sprite = sprite, text = displayString });
            }
        }
    }
}
