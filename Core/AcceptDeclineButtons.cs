﻿using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kiskovi.Core
{
    internal class AcceptDeclineButtons : MonoBehaviour
    {
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button cancelButton;
        private UIPanel parent;

        [Inject] private SignalBus signalBus;

        private void Awake()
        {
            parent = GetComponentInParent<UIPanel>();
        }

        private void OnEnable()
        {
            signalBus.Subscribe<UIInteractions.AccepSignal>(OnAccept);
            signalBus.Subscribe<UIInteractions.DeclineSignal>(OnDecline);
        }

        private void OnDisable()
        {
            signalBus.TryUnsubscribe<UIInteractions.AccepSignal>(OnAccept);
            signalBus.TryUnsubscribe<UIInteractions.DeclineSignal>(OnDecline);
        }

        public void OnAccept()
        {
            if (acceptButton != null && acceptButton.isActiveAndEnabled && acceptButton.interactable && (parent == null || parent.isInFront))
                acceptButton.onClick.Invoke();
        }

        public void OnDecline()
        {
            if (cancelButton != null && cancelButton.isActiveAndEnabled && cancelButton.interactable && (parent == null || parent.isInFront))
                cancelButton.onClick.Invoke();
        }
    }
}
