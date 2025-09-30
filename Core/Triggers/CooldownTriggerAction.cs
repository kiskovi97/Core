using UnityEngine;

namespace Kiskovi.Core
{
    internal class CooldownTriggerAction : TriggerAction
    {
        public float coolDown = 30;
        public TriggerAction onActivated;
        private float _coolDownTime;

        public override void Trigger(params object[] parameter)
        {
            base.Trigger(parameter);

            if (_coolDownTime > 0) return;

            Trigger(onActivated);

            _coolDownTime = coolDown;
        }
        private void Update()
        {
            if (_coolDownTime > 0)
                _coolDownTime -= Time.deltaTime;
        }
    }
}
