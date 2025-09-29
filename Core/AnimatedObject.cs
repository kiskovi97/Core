using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kiskovi.Core
{
    public class AnimatedObject : MonoBehaviour
    {
        [Header("Hide trigger: onHide")]
        [Header("Show trigger: onShow")]
        [Header("Destroy trigger: onDestroy")]
        [Space]
        [SerializeField] private float hideTime = 0.5f;
        [SerializeField] private float destroyTime = 0.5f;
        [SerializeField] private List<Animator> animators = new List<Animator>();
        [SerializeField] private bool unscaledTime;

        private bool prevValue = false;
        private bool isDestroying = false;

        public bool PrevValue => prevValue;

        private void Awake()
        {
            prevValue = true;
        }

        public void SetActive(bool active, bool instant = false)
        {
            if (instant)
            {
                gameObject.SetActive(active);
                return;
            }
            if (active)
            {
                StopAllCoroutines();
                gameObject.SetActive(true);
                SetTrigger("onShow");
            }
            else if (prevValue != active)
            {
                if (gameObject.activeInHierarchy)
                    StartCoroutine(SetActiveFalse());
                else
                    gameObject.SetActive(false);
            }
            prevValue = active;
        }

        public void Destroy()
        {
            if (isDestroying) return;
            isDestroying = true;

            StopAllCoroutines();
            StartCoroutine(DestroyAnimation());
        }

        private IEnumerator DestroyAnimation()
        {
            SetTrigger("onDestroy");
            if (unscaledTime)
                yield return new WaitForSecondsRealtime(destroyTime);
            else
                yield return new WaitForSeconds(destroyTime);

            Destroy(gameObject);
        }

        private IEnumerator SetActiveFalse()
        {
            SetTrigger("onHide");
            if (unscaledTime)
                yield return new WaitForSecondsRealtime(hideTime);
            else
                yield return new WaitForSeconds(hideTime);

            gameObject.SetActive(false);
        }

        private void SetTrigger(string trigger)
        {
            foreach (var animator in animators)
                if (animator != null && animator.gameObject.activeInHierarchy)
                {
                    animator.SetTrigger(trigger);
                    if (trigger != "onDestroy")
                        animator.ResetTrigger("onDestroy");
                    if (trigger != "onHide")
                        animator.ResetTrigger("onHide");
                    if (trigger != "onShow")
                        animator.ResetTrigger("onShow");
                }
        }
    }
}