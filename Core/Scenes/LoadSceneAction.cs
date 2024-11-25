using Zenject;

namespace Kiskovi.Core
{
    public class LoadSceneAction : TriggerAction
    {
        public int index;
        public bool force = false;
        public float delayTime = 0f;

        [Inject] private SignalBus _signalBus;

        public static void Trigger(TriggerAction action, params object[] parameter)
        {
            _signalBus.TryFire(new SceneLoadRequestSignal()
            {
                index = index,
                force = force,
                delayTime = delayTime,
            });
        }
    }
}