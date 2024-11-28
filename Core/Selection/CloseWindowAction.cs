namespace Kiskovi.Core
{
    internal class CloseWindowAction : TriggerAction
    {
        public UIWindow window;

        public override void Trigger(params object[] parameter)
        {
            if (window != null)
                window.Close();
        }
    }
}
