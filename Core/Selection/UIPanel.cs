using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Kiskovi.Core
{
    public class UIPanel : MonoBehaviour
    {
        public bool isUINavigationBlocked;

        public static event Action onChanged;

        protected List<UIPanel> connectedPanels = new List<UIPanel>();

        protected bool _isInFront;
        public bool IsBaseInFront => _isInFront;
        public virtual bool isInFront { get => _isInFront && !connectedPanels.Any(panel => panel.isInFront); protected set => _isInFront = value; }

        public virtual bool isOpened { get; protected set; }

        internal void ConnectPanel(UIPanel panel) { connectedPanels.Add(panel); }

        internal void DisconnectPanel(UIPanel panel) { connectedPanels.Remove(panel); }

        protected virtual void OnOpened() { Debug.Log("OnOpened " + name); isOpened = true; onChanged?.Invoke(); }

        protected virtual void OnClosed() { Debug.Log("OnClosed " + name); isOpened = false; onChanged?.Invoke(); }

        protected virtual void OnBackground() { Debug.Log("OnBackground " + name); isInFront = false; onChanged?.Invoke(); }

        protected virtual void OnFront() { Debug.Log("OnFront " + name); isInFront = true; onChanged?.Invoke(); }
    }
}
