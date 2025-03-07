using Zenject;

namespace Kiskovi.Core
{
    internal class ClearTutorialAction : TriggerAction
    {
        [Inject] private ITutorialManager _manager;

        public override void Trigger(params object[] parameter)
        {
            _manager.ClearTutorials();
        }
    }
}
