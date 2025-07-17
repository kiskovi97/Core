using System.Collections.Generic;
using System.Linq;

using Unity.Collections;

using UnityEngine;

namespace Kiskovi.Core
{
    public class InteractionObject : MonoBehaviour
    {
        public ContactFilter2D contactFilter;
        public Transform center;

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
                RecalculateObjects();
            }
            UpdateNearest();
        }

        public void RecalculateObjects()
        {
            //gameObject.SetActive(false);
            //gameObject.SetActive(true);

            objects.Clear();

            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

            foreach (var collider in colliders)
            {
                if (!collider.isTrigger) continue;

                Collider2D[] results = new Collider2D[100];
                int count = collider.Overlap(contactFilter, results);

                for (int i = 0; i < count; i++)
                {
                    if (colliders.Contains(results[i])) continue;

                    var interactable = results[i].gameObject.GetComponent<InteractableObject>();
                    if (interactable != null && !objects.Contains(interactable))
                    {
                        objects.Add(interactable);
                    }
                }
            }

            UpdateNearest();
        }

        private void UpdateNearest()
        {
            var prevNear = nearestObject;
            var currentCenter = center ?? transform;
            nearestObject = objects.Where(item => item != null).OrderBy(item => (item.transform.position - currentCenter.position).sqrMagnitude).FirstOrDefault();

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
