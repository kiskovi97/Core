namespace Kiskovi.Core
{
    internal class GameActionList : GameAction
    {
        public GameAction[] Actions;
        public override void Trigger(params object[] parameter)
        {
            foreach (var action in Actions)
            {
                Trigger(action);
            }
        }
    }
}
