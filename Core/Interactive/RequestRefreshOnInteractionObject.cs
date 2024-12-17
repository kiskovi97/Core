namespace Kiskovi.Core
{
    internal class RequestRefreshOnInteractionObject : TriggerAction
    {
        public InteractionObject target;

        public override void Trigger(params object[] parameter)
        {
            target.RequestRefresh();
        }
    }
}
