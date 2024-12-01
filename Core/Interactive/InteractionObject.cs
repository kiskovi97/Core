using System;
using System.Collections.Generic;
using System.Linq;

using Unity.Collections;

using UnityEngine;

namespace Kiskovi.Core
{
    public class InteractionObject : MonoBehaviour
    {
        private HashSet<InteractableObject> objects = new HashSet<InteractableObject>();

        [ReadOnly] public InteractableObject nearestObject;

        private int refresh;

        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.gameObject.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                objects.Add(interactable);
            }
            UpdateNearest();
        }

        private void OnTriggerExit(Collider other)
        {
            var interactable = other.gameObject.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                objects.Remove(interactable);
            }
            UpdateNearest();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = other.gameObject.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                objects.Add(interactable);
            }
            UpdateNearest();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var interactable = other.gameObject.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                objects.Remove(interactable);
            }
            UpdateNearest();
        }
        private void OnDisable()
        {
            objects.Clear();
            UpdateNearest();
        }

        private void Update()
        {
            if (refresh > 0)
            {
                refresh--;
            }
            else if (refresh == 0)
            {
                refresh = -1;
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
            UpdateNearest();
        }

        private void UpdateNearest()
        {
            var prevNear = nearestObject;
            nearestObject = objects.OrderBy(item => (item.transform.position - transform.position).sqrMagnitude).FirstOrDefault();

            if (prevNear == nearestObject) return;

            if (prevNear != null)
            {
                prevNear.RemoveNearest(this);
            }

            if (nearestObject != null)
            {
                nearestObject.AddNearest(this);
            }
        }

        public void RequestRefresh()
        {
            refresh = 3;
        }
    }
}
