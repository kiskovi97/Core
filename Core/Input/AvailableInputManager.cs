using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

using static Kiskovi.Core.InputSignals;

namespace Kiskovi.Core
{

    public interface IAvailableInputManager
    {
        IEnumerable<InputInfoGroup> AvailableInputs { get; }

        void RegisterInput(InputInfoGroup input, MonoBehaviour reference);
        void DeRegisterInput(InputInfoGroup input, MonoBehaviour reference);

        event Action OnChanged;
    }

    internal class AvailableInputManager : IAvailableInputManager
    {
        struct InputInfoDependency
        {
            public InputInfoGroup info;
            public MonoBehaviour dependent;
        }

        private readonly List<InputInfoDependency> _availableInputs = new List<InputInfoDependency>();

        public event Action OnChanged;

        public IEnumerable<InputInfoGroup> AvailableInputs => _availableInputs.Select(item => item.info).Distinct();

        public AvailableInputManager(SignalBus signalBus)
        {
            signalBus.Subscribe<ControlSchemeChanged>(SendChanged);
        }

        private void SendChanged()
        {
            OnChanged?.Invoke();
        }

        public void DeRegisterInput(InputInfoGroup inputGroup, MonoBehaviour reference)
        {
            var dependency = new InputInfoDependency()
            {
                info = inputGroup,
                dependent = reference
            };
            if (_availableInputs.Contains(dependency))
            {
                _availableInputs.Remove(dependency);

                OnChanged?.Invoke();
            }
        }

        public void RegisterInput(InputInfoGroup inputGroup, MonoBehaviour reference)
        {
            var dependency = new InputInfoDependency()
            {
                info = inputGroup,
                dependent = reference
            };
            if (!_availableInputs.Contains(dependency))
            {
                _availableInputs.Add(dependency);

                OnChanged?.Invoke();
            }
        }
    }
}
