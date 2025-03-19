using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Kiskovi.Core
{
    public class UIWindow : UIPanel
    {
        [Header("show trigger: open")]
        [Header("show trigger: close")]
        [Space]

        [SerializeField] private Animator animator;
        [SerializeField] protected GameObject[] windowObjects;
        [SerializeField] private GameObject blockingObject;
        [SerializeField] private TriggerAction onOpen;
        [SerializeField] private TriggerAction onClose;
        [SerializeField] private float closeAnimationTime = 0.2f;

        private static List<UIWindow> openedWindows = new List<UIWindow>();

        private static float animationTime = 0.2f;

        public static UIWindow PauseMenu { get; set; }

        public static bool IsWindowOpen => openedWindows.Count > 0;

        public bool isOpen => windowObjects != null && windowObjects.Length > 0 && windowObjects.Any(item => item.activeInHierarchy);

        private static UIWindow inProgress = null;

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
            if (inProgress != null) return;
            if (openedWindows.Count > 0)
            {
                var window = openedWindows.LastOrDefault();
                window.Close();
            }
            else
            {
                if (PauseMenu != null)
                    PauseMenu.Open();
            }
        }

        public void Open()
        {
            if (!isOpen)
            {
                Debug.Log("Open " + name);
                StartCoroutine(StartToOpen());
            }
        }

        public virtual void Close()
        {
            if (isOpen)
            {
                StartCoroutine(StartToClose());
            }
        }

        public void GoToBackground(bool blockUI = true)
        {
            StartCoroutine(StartToGoBackground(blockUI));
        }

        public void GoToFront()
        {
            StartCoroutine(StartToGoFront());
        }

        private IEnumerator StartToGoBackground(bool blockUI)
        {
            OnBackground();
            blockingObject.SetObjectActive(blockUI);
            inProgress = this;
            if (animator != null)
            {
                //animator.SetTrigger("background");
                yield return new WaitForSecondsRealtime(animationTime);
            }
            else
            {
                yield return null;
            }
            inProgress = null;
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
            OnFront();
            inProgress = this;
            if (animator != null)
            {
                //animator.SetTrigger("front");
                yield return new WaitForSecondsRealtime(animationTime);
            }
            else
            {
                yield return null;
            }
            inProgress = null;
            if (blockingObject != null)
                blockingObject.SetObjectActive(false);
        }

        private IEnumerator StartToOpen()
        {
            if (openedWindows.Count > 0)
            {
                var last = openedWindows.LastOrDefault();
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
            openedWindows = openedWindows.Append(this).OrderBy(item => item.transform.GetSiblingIndex()).ToList();
            OnFront();
            OnOpened();

            inProgress = this;
            if (animator != null)
            {
                animator.SetTrigger("open");
                yield return new WaitForSecondsRealtime(animationTime);
            }
            else
            {
                yield return null;
            }
            inProgress = null;

            if (blockingObject != null)
                blockingObject.SetObjectActive(false);
        }

        private IEnumerator StartToClose()
        {
            if (openedWindows.Count == 0)
                yield break;
            OnBackground();

            openedWindows.Remove(this);
            if (blockingObject != null)
                blockingObject.SetObjectActive(true);

            TriggerAction.Trigger(onClose);

            inProgress = this;
            if (animator != null)
            {
                animator.SetTrigger("close");
                yield return new WaitForSecondsRealtime(closeAnimationTime);
            }
            else
            {
                yield return null;
            }
            inProgress = null;

            foreach (var window in windowObjects)
            {
                if (window != null)
                    window.SetObjectActive(false);
            }
            if (blockingObject != null)
                blockingObject.SetObjectActive(false);

            if (openedWindows.Count > 0)
            {
                var last = openedWindows.LastOrDefault();
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
