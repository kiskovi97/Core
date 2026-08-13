using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kiskovi.Core
{
    /// <summary>
    /// Abstract base class for UI windows. Supports both Canvas-based (UIWindow) and UIToolkit-based (UIToolkitWindow) implementations.
    /// Both types share a common window stack (openedWindows) for consistent focus/state management.
    /// Static methods (CloseLast, OpenPause) work with windows of any type.
    /// </summary>
    public abstract class UIWindowBase : UIPanel
    {
        // Shared static state used by all UIWindowBase implementations
        protected static List<UIWindowBase> openedWindows = new List<UIWindowBase>();
        protected static float animationTime = 0.2f;
        protected static UIWindowBase inProgress = null;

        public static UIWindowBase PauseMenu { get; set; }

        /// <summary>
        /// Returns true if any window is currently open (Canvas or UIToolkit)
        /// </summary>
        public static bool IsWindowOpen => openedWindows.Count > 0;

        public abstract void Close();
        public abstract void Open();
        public abstract void GoToBackground(bool blockUI = true);
        public abstract void GoToFront();

        /// <summary>
        /// Adds a window to the shared openedWindows list, ordered by sibling index
        /// </summary>
        protected static void AddToOpenedWindows(UIWindowBase window)
        {
            openedWindows = openedWindows
                .Append(window)
                .OrderBy(item => item.transform.GetSiblingIndex())
                .ToList();
        }

        /// <summary>
        /// Removes a window from the shared openedWindows list
        /// </summary>
        protected static void RemoveFromOpenedWindows(UIWindowBase window)
        {
            openedWindows.Remove(window);
        }

        /// <summary>
        /// Gets the last (topmost) window in the stack
        /// </summary>
        protected static UIWindowBase GetLastOpenedWindow()
        {
            return openedWindows.LastOrDefault();
        }

        /// <summary>
        /// Checks if any window transition is currently in progress
        /// </summary>
        protected static bool IsInProgress()
        {
            return inProgress != null;
        }

        /// <summary>
        /// Sets the current window as in-progress (prevents other operations)
        /// </summary>
        protected static void SetInProgress(UIWindowBase window)
        {
            inProgress = window;
        }

        /// <summary>
        /// Clears the in-progress flag after a transition completes
        /// </summary>
        protected static void ClearInProgress()
        {
            inProgress = null;
        }
    }

    public class UIWindow : UIWindowBase
    {
        [Header("show trigger: open")]
        [Header("show trigger: close")]
        [Space]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        protected GameObject[] windowObjects;

        [SerializeField]
        private GameObject blockingObject;

        [SerializeField]
        private TriggerAction onOpen;

        [SerializeField]
        private TriggerAction onClose;

        [SerializeField]
        private float closeAnimationTime = 0.2f;

        public bool isOpen =>
            windowObjects != null
            && windowObjects.Length > 0
            && windowObjects.Any(item => item.activeInHierarchy);

        protected virtual void Start()
        {
            foreach (var window in windowObjects)
            {
                if (window != null)
                    window.SetObjectActive(false);
            }
            if (blockingObject != null)
                blockingObject.SetObjectActive(false);
        }

        protected virtual void OnDestroy()
        {
            if (openedWindows.Contains(this))
                openedWindows.Clear();
        }

        public static void CloseLast()
        {
            if (IsInProgress())
                return;
            if (openedWindows.Count > 0)
            {
                var window = GetLastOpenedWindow();
                window.Close();
            }
            else
            {
                if (PauseMenu != null)
                    PauseMenu.Open();
            }
        }

        public static void OpenPause()
        {
            if (IsInProgress())
                return;
            var windows = openedWindows.ToList();
            foreach (var window in windows)
            {
                window.Close();
            }
            if (PauseMenu != null)
                PauseMenu.Open();
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
            blockingObject.SetObjectActive(blockUI);
            SetInProgress(this);
            if (animator != null)
            {
                //animator.SetTrigger("background");
                yield return new WaitForSecondsRealtime(animationTime);
            }
            else
            {
                yield return null;
            }
            ClearInProgress();
            //if (windowObject != null)
            //    windowObject.SetObjectActive(false);
        }

        private IEnumerator StartToGoFront()
        {
            foreach (var window in windowObjects)
            {
                if (window != null)
                    window.SetObjectActive(true);
            }
            if (blockingObject != null)
                blockingObject.SetObjectActive(true);
            SetInProgress(this);
            if (animator != null)
            {
                //animator.SetTrigger("front");
                yield return new WaitForSecondsRealtime(animationTime);
            }
            else
            {
                yield return null;
            }
            OnFront();
            ClearInProgress();
            if (blockingObject != null)
                blockingObject.SetObjectActive(false);
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

            foreach (var window in windowObjects)
            {
                if (window != null)
                    window.SetObjectActive(true);
            }
            if (blockingObject != null)
                blockingObject.SetObjectActive(true);

            TriggerAction.Trigger(onOpen);
            AddToOpenedWindows(this);
            OnOpened();

            SetInProgress(this);
            if (animator != null)
            {
                animator.SetTrigger("open");
                yield return new WaitForSecondsRealtime(animationTime);
            }
            else
            {
                yield return null;
            }
            OnFront();
            ClearInProgress();

            if (blockingObject != null)
                blockingObject.SetObjectActive(false);
        }

        private IEnumerator StartToClose()
        {
            if (openedWindows.Count == 0 || inProgress == this)
                yield break;
            OnBackground();

            RemoveFromOpenedWindows(this);
            if (blockingObject != null)
                blockingObject.SetObjectActive(true);

            TriggerAction.Trigger(onClose);

            SetInProgress(this);
            if (animator != null)
            {
                animator.SetTrigger("close");
                yield return new WaitForSecondsRealtime(closeAnimationTime);
            }
            else
            {
                yield return null;
            }
            ClearInProgress();

            foreach (var window in windowObjects)
            {
                if (window != null)
                    window.SetObjectActive(false);
            }
            if (blockingObject != null)
                blockingObject.SetObjectActive(false);

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
