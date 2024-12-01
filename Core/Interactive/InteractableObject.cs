using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Kiskovi.Core
{
    public class InteractableObject : MonoBehaviour
    {
        private HashSet<GameObject> players = new HashSet<GameObject>();
        private HashSet<InteractionObject> interactions = new HashSet<InteractionObject>();

        public GameObject nearObject;
        public GameObject nearestObject;
        public GameObject grabbedObject;
        public bool grabbed;

        public GameObject ClosestObject => interactions.FirstOrDefault()?.gameObject;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                players.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                players.Remove(other.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                players.Add(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                players.Remove(other.gameObject);
            }
        }

        private void OnDisable()
        {
            players.Clear();
            interactions.Clear();

            if (nearObject != null)
                nearObject.SetActive(grabbed);
            if (nearestObject != null)
                nearestObject.SetActive(false);
            if (grabbedObject != null)
                grabbedObject.SetActive(false);
        }

        private void Update()
        {
            if (nearObject != null)
                nearObject.SetActive(players.Any() || grabbed);
            if (nearestObject != null)
                nearestObject.SetActive(interactions.Any());
            if (grabbedObject != null)
                grabbedObject.SetActive(grabbed);
        }

        internal void AddNearest(InteractionObject interactionObject)
        {
            interactions.Add(interactionObject);
        }

        internal void RemoveNearest(InteractionObject interactionObject)
        {
            interactions.Remove(interactionObject);
        }
    }
}
