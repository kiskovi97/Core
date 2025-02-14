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
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<UIInteractions.ExitSignal>(OnExit);
        }

        private void OnExit()
        {
            UIWindow.CloseLast();
        }
    }
}
