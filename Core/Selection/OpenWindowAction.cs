namespace Kiskovi.Core
{
    internal class OpenWindowAction : TriggerAction
    {
        public UIWindow window;

        public override void Trigger(params object[] parameter)
        {
            if (window != null)
                window.Open();
        }
    }
}
