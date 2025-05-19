using UnityEngine;

namespace Kiskovi.Core
{
    public enum ControlScheme
    {
        Keyboard,
        XboxController
    }
    public class MoveSignal
    {
        public Vector2 Move;

        public MoveSignal(Vector2 move)
        {
            Move = move;
        }
    }

    public class UIInteractions
    {
        public class ExitSignal { }
        public class AcceptSignal { }
        public class DeclineSignal { }
        public class DeleteSignal { }
        public class Navigate
        {
            public Vector2 Delta;

            public Navigate(Vector2 delta)
            {
                Delta = delta;
            }
        }
        public class NavigateUI
        {
            public Vector2 Delta;

            public NavigateUI(Vector2 delta)
            {
                Delta = delta;
            }
        }
        public class NavigateTabsSignal
        {
            public bool forward;

            public NavigateTabsSignal(bool forward)
            {
                this.forward = forward;
            }
        }
        public class ModifyValueSignal
        {
            public float Delta;

            public ModifyValueSignal(float delta)
            {
                Delta = delta;
            }
        }
    }

    public class InputSignals
    {
        public static ControlScheme Scheme = ControlScheme.Keyboard;
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
