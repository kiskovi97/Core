using UnityEngine;

namespace Kiskovi.Core
{
    internal class ChangeParticleSystemAction : TriggerAction
    {
        public ParticleSystem myParticleSystem;
        public ParticleSystemChange change;
        public enum ParticleSystemChange
        {
            Play,
            Stop,
        }

        public override void Trigger(params object[] parameter)
        {
            switch(change)
            {
                case ParticleSystemChange.Play:
                    myParticleSystem.Play();
                    break;
                case ParticleSystemChange.Stop:
                    myParticleSystem.Stop();
                    break;
            }
        }

    }
}
