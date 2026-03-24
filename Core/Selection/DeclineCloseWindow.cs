using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kiskovi.Core
{
    internal class DeclineCloseWindow : MonoBehaviour
    {
        [SerializeField]
        private UIWindow closableWindow;

        [Inject]
        private SignalBus signalBus;

        private void OnEnable()
        {
            signalBus.Subscribe<UIInteractions.DeclineSignal>(OnDecline);
        }

        private void OnDisable()
        {
            signalBus.TryUnsubscribe<UIInteractions.DeclineSignal>(OnDecline);
        }

        public void OnDecline()
        {
            if (closableWindow != null && closableWindow.isInFront)
                closableWindow.Close();
        }
    }
}
