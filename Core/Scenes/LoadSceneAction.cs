using Zenject;

namespace Kiskovi.Core
{
    public class LoadSceneAction : TriggerAction
    {
        public SceneEnum index;
        public bool force = false;
        public float delayTime = 0f;

        [Inject] private SignalBus _signalBus;

        public override void Trigger(params object[] parameter)
        {
            _signalBus.TryFire(new SceneLoadRequestSignal()
            {
                scene = index,
                force = force,
                delayTime = delayTime,
            });
        }
    }
}