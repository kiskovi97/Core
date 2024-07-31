namespace Kiskovi.Core
{
    internal class TriggerActionList : TriggerAction
    {
        public TriggerAction[] Actions;
        public override void Trigger(params object[] parameter)
        {
            foreach (var action in Actions)
            {
                Trigger(action);
            }
        }
    }
}
