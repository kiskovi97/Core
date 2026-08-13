using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kiskovi.Core
{
    /// <summary>
    /// UIToolkit-based window implementation. Provides basic window lifecycle management
    /// without animation. Shares window stack with Canvas-based UIWindow implementations.
    /// </summary>
    public class UIToolkitWindow : UIWindowBase
    {
        [SerializeField]
        private UIDocument uIDocument;

        [SerializeField]
        private TriggerAction onOpen;

        [SerializeField]
        private TriggerAction onClose;

        public bool isOpen =>
            uIDocument != null
            && uIDocument.rootVisualElement != null
            && uIDocument.rootVisualElement.style.display != DisplayStyle.None;

        protected virtual void Start()
        {
            if (uIDocument != null && uIDocument.rootVisualElement != null)
            {
                uIDocument.rootVisualElement.style.display = DisplayStyle.None;
            }
        }

        protected virtual void OnDestroy()
        {
            if (openedWindows.Contains(this))
                openedWindows.Remove(this);
        }

        public override void Open()
        {
            if (!isOpen)
            {
                Debug.Log("Open " + name);
                StartCoroutine(StartToOpen());
            }
        }

        public override void Close()
        {
            if (isOpen)
            {
                StartCoroutine(StartToClose());
            }
        }

        public override void GoToBackground(bool blockUI = true)
        {
            StartCoroutine(StartToGoBackground(blockUI));
        }

        public override void GoToFront()
        {
            StartCoroutine(StartToGoFront());
        }

        private IEnumerator StartToGoBackground(bool blockUI)
        {
            OnBackground();
            SetInProgress(this);
            // UIToolkit windows don't animate, yield one frame for consistency
            yield return null;
            ClearInProgress();
        }

        private IEnumerator StartToGoFront()
        {
            if (uIDocument != null && uIDocument.rootVisualElement != null)
            {
                uIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
            }
            SetInProgress(this);
            yield return null;
            OnFront();
            ClearInProgress();
        }

        private IEnumerator StartToOpen()
        {
            if (openedWindows.Count > 0)
            {
                var last = GetLastOpenedWindow();
                if (last != null)
                    last.GoToBackground();
            }
            if (UIBasePanel.Instance != null)
                UIBasePanel.Instance.ToBack();

            if (uIDocument != null && uIDocument.rootVisualElement != null)
            {
                uIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
            }

            TriggerAction.Trigger(onOpen);
            AddToOpenedWindows(this);
            OnOpened();

            SetInProgress(this);
            yield return null;
            OnFront();
            ClearInProgress();
        }

        private IEnumerator StartToClose()
        {
            if (openedWindows.Count == 0 || inProgress == this)
                yield break;
            OnBackground();

            RemoveFromOpenedWindows(this);
            TriggerAction.Trigger(onClose);

            SetInProgress(this);
            yield return null;
            ClearInProgress();

            if (uIDocument != null && uIDocument.rootVisualElement != null)
            {
                uIDocument.rootVisualElement.style.display = DisplayStyle.None;
            }

            if (openedWindows.Count > 0)
            {
                var last = GetLastOpenedWindow();
                if (last != null)
                    last.GoToFront();
                else if (UIBasePanel.Instance != null)
                    UIBasePanel.Instance.ToFront();
            }
            else if (UIBasePanel.Instance != null)
                UIBasePanel.Instance.ToFront();

            OnClosed();
        }
    }
}
