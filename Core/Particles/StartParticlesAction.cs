namespace Kiskovi.Core
{
    internal class StartParticlesAction : TriggerAction
    {
        public ParticleTimer particleTimer;
        public float time = 1f;

        public override void Trigger(params object[] parameter)
        {
            particleTimer.AddTime(time);
        }
    }
}
