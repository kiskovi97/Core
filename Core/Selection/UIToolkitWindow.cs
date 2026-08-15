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

        private VisualElement rootPanel;

        [SerializeField]
        private float closeAnimationTime = 0.3f;

        private bool _isOpen;

        public bool isOpen => _isOpen;

        private const string HiddenClass = "window-hidden";

        protected virtual void Start()
        {
            if (uIDocument == null)
                return;

            rootPanel = uIDocument.rootVisualElement.Q<VisualElement>("Base");

            if (rootPanel == null)
            {
                Debug.LogError($"UIToolkitWindow '{name}' could not find VisualElement 'Base'.");
                return;
            }

            HideInstant();
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
                yield return Show();
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

            TriggerAction.Trigger(onOpen);
            AddToOpenedWindows(this);
            OnOpened();
            SetInProgress(this);

            if (uIDocument != null && uIDocument.rootVisualElement != null)
            {
                yield return Show();
            }

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

            if (uIDocument != null && uIDocument.rootVisualElement != null)
            {
                yield return Hide();
            }

            ClearInProgress();

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

        private IEnumerator Show()
        {
            rootPanel.style.display = DisplayStyle.Flex;

            rootPanel.RemoveFromClassList(HiddenClass);

            yield return null;

            _isOpen = true;
        }

        private void HideInstant()
        {
            rootPanel.AddToClassList(HiddenClass);

            rootPanel.style.display = DisplayStyle.None;
        }

        private IEnumerator Hide()
        {
            rootPanel.AddToClassList(HiddenClass);

            yield return new WaitForSecondsRealtime(closeAnimationTime);

            rootPanel.style.display = DisplayStyle.None;

            _isOpen = false;
        }
    }
}
