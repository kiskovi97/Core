using UnityEngine;

namespace Kiskovi.Core
{
    public enum ControlScheme
    {
        Keyboard,
        XboxController
    }
    public class MoveSignal : InputVector2Signal
    {
        public MoveSignal(Vector2 move) : base(move) { }
    }

    public class UIInteractions
    {
        public class ExitSignal : InputSimpleSignal { }

        public class AcceptSignal : InputSimpleSignal { }

        public class DeclineSignal : InputSimpleSignal { }

        public class DeleteSignal : InputSimpleSignal { }
        public class Navigate : InputVector2Signal
        {
            public Navigate(Vector2 delta) : base(delta) { }
        }
        public class NavigateUI : InputVector2Signal
        {
            public NavigateUI(Vector2 delta) : base(delta) { }
        }
        public class NavigateTabsSignal : InputBooleanSignal
        {
            public NavigateTabsSignal(bool forward) : base(forward) { }
        }
        public class ModifyValueSignal : InputFloatSignal
        {
            public ModifyValueSignal(float value) : base(value) { }
        }

        public class SkipSignal : InputSimpleSignal { }
    }

    public class InputSignals
    {
        public const string KEYBOARD_NAME = "Keyboard";
        public const string XBOX_NAME = "XboxController";

        public static ControlScheme Scheme = ControlScheme.Keyboard;
        public static string SchemeName => (Scheme) switch { 
            ControlScheme.Keyboard => KEYBOARD_NAME, 
            ControlScheme.XboxController => XBOX_NAME, 
            _ => KEYBOARD_NAME 
        };
        public static string translationKey => (Scheme) switch
        {
            ControlScheme.Keyboard => "_keyboard",
            ControlScheme.XboxController => "_xbox",
            _ => "_keyboard",
        };

        public class ControlSchemeChanged
        {
            public ControlScheme Scheme;
            public ControlSchemeChanged(ControlScheme scheme)
            {
                Scheme = scheme;
            }
        }
    }
}
