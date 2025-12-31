using System;
using UnityEngine;
using Zenject;

namespace Kiskovi.Core
{
    public class InputSignalReciver : MonoBehaviour
    {
        public TriggerAction onSignalRecived;

        [HideInInspector] public string inputActionSignalTypeName;

        private Type _cachedType;
        protected Type CachedType
        {
            get
            {
                if (_cachedType == null)
                    _cachedType = InputActionSignalTypeCache.GetByName(inputActionSignalTypeName);
                return _cachedType;
            }
        }

        [Inject] protected SignalBus _signalBus;

        private void OnEnable()
        {
            _signalBus.Subscribe(CachedType, SignalCalled);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe(CachedType, SignalCalled);
        }

        private void SignalCalled(object signal)
        {
            TriggerAction.Trigger(onSignalRecived);
        }
    }
}
