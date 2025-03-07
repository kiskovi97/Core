using Zenject;

namespace Kiskovi.Core
{
    internal class CompleteTutorial : TriggerAction
    {
        public TutorialReference tutorial;

        [Inject] private ITutorialManager _manager;

        public override void Trigger(params object[] parameter)
        {
            _manager.CompleteTutorial(tutorial);
        }
    }
}
