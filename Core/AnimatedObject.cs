using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kiskovi.Core
{
    public class AnimatedObject : MonoBehaviour
    {
        [Header("Destroy trigger: onHide")]
        [Space]
        [SerializeField] private float destroyTime = 0.5f;
        [SerializeField] private List<Animator> animators = new List<Animator>();
        [SerializeField] private bool unscaledTime;

        private bool prevValue = false;

        public bool PrevValue => prevValue;

        private void Start()
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
                gameObject.SetActive(true);
            else if (prevValue != active)
            {
                if (gameObject.activeInHierarchy)
                    StartCoroutine(SetActiveFalse());
                else
                    gameObject.SetActive(false);
            }
            prevValue = active;
        }

        private IEnumerator SetActiveFalse()
        {
            foreach (var animator in animators)
                if (animator.gameObject.activeInHierarchy)
                    animator.SetTrigger("onHide");
            if (unscaledTime)
                yield return new WaitForSecondsRealtime(destroyTime);
            else
                yield return new WaitForSeconds(destroyTime);

            gameObject.SetActive(false);
        }
    }
}