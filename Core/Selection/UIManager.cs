using System;
using Zenject;

namespace Kiskovi.Core
{
    internal class UIManager : IInitializable, IDisposable
    {
        private SignalBus _signalBus;

        public UIManager(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<UIInteractions.ExitSignal>(OnExit);
            _signalBus.Subscribe<PauseGameRequestSignal>(OnPauseGameRequest);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<UIInteractions.ExitSignal>(OnExit);
            _signalBus.Unsubscribe<PauseGameRequestSignal>(OnPauseGameRequest);
        }

        private void OnExit()
        {
            UIWindow.CloseLast();
        }

        private void OnPauseGameRequest()
        {
            UIWindow.OpenPause();
        }
    }
}
